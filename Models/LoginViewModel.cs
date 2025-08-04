using System.ComponentModel.DataAnnotations;

namespace OnlineShopping_BIT_2025.Models
{
    public class LoginViewModel
    {
        [DataType(DataType.EmailAddress)]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
