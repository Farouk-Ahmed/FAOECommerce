
namespace CleanArchitecture.DataAccess.Models
{
    public class Otp : ModelBase
    {
        public string Key { get; set; }
        [Required]
        public string Code { get; set; }
        public DateTime ExpiresAt { get; set; }
        [Required]
        public string UserName { get; set; }
        public string UserId { get; set; } 
        public bool IsUsed { get; set; } = false;
    }
}
