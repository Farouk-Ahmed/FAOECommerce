namespace CleanArchitecture.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IAuthService _authServices;
        private readonly IOtpService _otpService;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IEmailService emailService,
            IConfiguration configuration,
            SignInManager<ApplicationUser> signInManager,
            IAuthService authServices,
            IOtpService otpService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _emailService = emailService;
            _configuration = configuration;
            _signInManager = signInManager;
            _authServices = authServices;
            _otpService = otpService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromForm] RegisterUser registerUser)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Generate OTP
            var otp = GenerateSimpleOtp();

            // Register user (do not send confirmation token)
            var response = await _authServices.RegisterUserAsync(registerUser);

            if (!response.IsSuccess)
                return BadRequest(response.Message ?? "User could not be created");

            // Save OTP in DB for this user
            var user = await _userManager.FindByEmailAsync(registerUser.Email);
            if (user == null)
                return BadRequest("User not found after registration.");
            await _otpService.SetOtpAsync(registerUser.Email, otp, user.UserName, user.Id, TimeSpan.FromMinutes(5));

            // Render confirmation email from HTML template, showing OTP
            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "EmailTemplates", "RegisterConfirmation.html");
            var htmlContent = RenderTemplate(templatePath, new Dictionary<string, string>
            {
                { "FirstName", registerUser.FirstName },
                { "LastName", registerUser.LastName },
                { "Otp", otp }
            });

            var message = new Message(
                new string[] { registerUser.Email! },
                "Confirm Your Email (OTP)",
                htmlContent
            );

            try
            {
                _emailService.SendEmail(message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Email error: " + ex.Message);
            }

            return Ok(response);
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string email, string otp)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return BadRequest(new Response { Status = "Error", Message = "User doesn't exist.", IsSuccess = false });

            var storedOtp = await _otpService.GetOtpAsync(email);
            if (storedOtp == null)
                return BadRequest(new Response { Status = "Error", Message = "OTP expired or not found.", IsSuccess = false });

            if (storedOtp != otp)
                return BadRequest(new Response { Status = "Error", Message = "Invalid OTP.", IsSuccess = false });

            user.EmailConfirmed = true;
            await _userManager.UpdateAsync(user);
            await _otpService.SetOtpAsUsedAsync(email);

            return Ok(new Response { Status = "Success", Message = "Email Verified Successfully.", IsSuccess = true });
        }

        [HttpPost("LogIn")]
        public async Task<IActionResult> LogIn([FromForm] SignInDTO signInDTO)
        {
            var user = await _userManager.FindByEmailAsync(signInDTO.Email);
            if (user == null)
                return Unauthorized(new Response { IsSuccess = false, Message = "User not found.", Status = "Error" });

            if (!user.EmailConfirmed)
                return Unauthorized(new Response { IsSuccess = false, Message = "Please confirm your email to login.", Status = "Error" });

            var passwordValid = await _userManager.CheckPasswordAsync(user, signInDTO.Password);
            if (!passwordValid)
                return Unauthorized(new Response { IsSuccess = false, Message = "Invalid credentials.", Status = "Error" });

            var tokenResponse = await _authServices.GetJwtTokenAsync(user);
            if (!tokenResponse.IsSuccess)
                return StatusCode(StatusCodes.Status500InternalServerError, tokenResponse);

            return Ok(tokenResponse);
        }

        [HttpPost("ForgetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgetPassword([FromForm] string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return Ok(new Response { IsSuccess = false, Message = "User not found.", Status = "Error" });

            var otp = GenerateSimpleOtp();

            // Save OTP in the database with user name
            await _otpService.SetOtpAsync(email, otp, user.UserName, user.Id, TimeSpan.FromMinutes(5));

            // Render OTP email from HTML template
            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "EmailTemplates", "Otp.html");
            var htmlContent = RenderTemplate(templatePath, new Dictionary<string, string>
            {
                { "FirstName", user.FirstName },
                { "LastName", user.LastName },
                { "Otp", otp }
            });

            var message = new Message(new string[] { user.Email! }, "Password Reset OTP", htmlContent);
            _emailService.SendEmail(message);

            return Ok(new Response { IsSuccess = true, Message = $"OTP sent to {user.Email}.", Status = "Success" });
        }

        [HttpPost("VerifyOtp")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyOtp([FromForm] VerifyOtpRequest request)
        {
            var storedOtp = await _otpService.GetOtpAsync(request.Email);

            if (storedOtp == null)
                return BadRequest("OTP expired or not found");

            if (storedOtp != request.Otp)
                return BadRequest("Invalid OTP");

            return Ok("OTP verified successfully");
        }

        [HttpPost("ResetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromForm] ResetPassword request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var storedOtp = await _otpService.GetOtpAsync(request.Email);

            if (storedOtp == null || storedOtp != request.Otp)
                return BadRequest(new { message = "OTP verification required or invalid" });

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return BadRequest(new Response { IsSuccess = false, Message = "OTP verification required, invalid, or already used.", Status = "Not Found" });

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, resetToken, request.NewPassword);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            await _otpService.SetOtpAsUsedAsync(request.Email);

            // Send password changed email
            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "EmailTemplates", "PasswordChanged.html");
            var htmlContent = RenderTemplate(templatePath, new Dictionary<string, string>
            {
                { "FirstName", user.FirstName },
                { "LastName", user.LastName }
            });
            var message = new Message(new string[] { user.Email! }, "Password Changed", htmlContent);
            _emailService.SendEmail(message);

            return Ok(new { message = "Password reset successfully" });
        }

        #region Private Methods
        private string GenerateSimpleOtp()
        {
            var random = new Random();
            return random.Next(1000, 9999).ToString();
        }

        private string RenderTemplate(string templatePath, Dictionary<string, string> values)
        {
            var template = System.IO.File.ReadAllText(templatePath, Encoding.UTF8);
            foreach (var pair in values)
            {
                template = template.Replace($"{{{{{pair.Key}}}}}", pair.Value);
            }
            return template;
        }
        #endregion
    }
}
