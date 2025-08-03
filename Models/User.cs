using System.ComponentModel.DataAnnotations;
namespace OnlineShopping_BIT_2025.Models
{
    public class User
    {
        public int Id { get; set; }

        [DataType(DataType.EmailAddress)]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [StringLength(20,MinimumLength =6, ErrorMessage = "Password length must be between 6-20")]
        public string Password { get; set; }

        public string Mobile { get; set; }

        public string Address { get; set; }

        public string? UserType { get; set; }
    }
}
