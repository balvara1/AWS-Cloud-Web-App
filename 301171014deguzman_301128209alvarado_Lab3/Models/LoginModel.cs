using System.ComponentModel.DataAnnotations;

namespace _301171014deguzman_301128209alvarado_Lab3.Models
{
    public class LoginModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your Username.")]
        public string UserId { get; set; }

        [DataType(DataType.Password)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your Password.")]
        public string Password { get; set; }
    }
}