using Microsoft.AspNetCore.Identity;

namespace SchoolConnect_DomainLayer.Models
{
    public class CustomIdentityUser : IdentityUser
    {
        public bool ResetPassword { get; set; }
    }
}
