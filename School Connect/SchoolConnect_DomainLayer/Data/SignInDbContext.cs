using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using SchoolConnect_DomainLayer.Models;

namespace SchoolConnect_DomainLayer.Data
{
    public class SignInDbContext : IdentityDbContext<CustomIdentityUser>
    {
        public SignInDbContext(DbContextOptions<SignInDbContext> options) : base(options) { }
    }
}
