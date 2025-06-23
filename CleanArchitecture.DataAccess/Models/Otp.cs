using System;
using System.ComponentModel.DataAnnotations;
using CleanArchitecture.DataAccess.Models;

namespace CleanArchitecture.DataAccess.Models
{
    public class Otp : ModelBase
    {
        [Required]
        public string Key { get; set; }
        [Required]
        public string Code { get; set; }
        public DateTime ExpiresAt { get; set; }
        [Required]
        public string UserName { get; set; } // New column to track who the OTP was sent to
    }
}
