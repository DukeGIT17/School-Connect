using SchoolConnect_DomainLayer.Models;
using System.ComponentModel.DataAnnotations;

namespace SchoolConnect_Web_App.Models
{
    public class LoginModel
    {
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
