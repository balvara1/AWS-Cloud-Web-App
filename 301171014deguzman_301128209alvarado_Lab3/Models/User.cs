using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace _301171014deguzman_301128209alvarado_Lab3.Models
{
    public partial class User
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your Username.")]
        public string UserId { get; set; }

        [DataType(DataType.Password)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your Password.")]
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
