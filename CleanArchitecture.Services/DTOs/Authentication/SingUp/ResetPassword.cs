
namespace BrainHope.Services.DTO.Authentication.SingUp
{
    public class ResetPassword
    {

        public string Email { get; set; }

        public string Otp { get; set; }


        public string NewPassword { get; set; }


        public string ConfirmNewPassword { get; set; }
    }


}
