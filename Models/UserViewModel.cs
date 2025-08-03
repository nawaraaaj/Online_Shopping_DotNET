using System.ComponentModel.DataAnnotations;

namespace OnlineShopping_BIT_2025.Models
{
    public class UserViewModel
    {
        [DataType(DataType.EmailAddress)]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Password length must be between 6-20")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Password length must be between 6-20")]
        [Compare("Password", ErrorMessage = "Password doesn't match.")]
        public string ConfirmPassword { get; set; }

        public string Mobile { get; set; }

        public string Address { get; set; }
    }
}
