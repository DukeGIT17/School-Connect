using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace SchoolConnect_DomainLayer.Data
{
    public class SignInDbContext : IdentityDbContext
    {
        public SignInDbContext(DbContextOptions<SignInDbContext> options) : base(options) { }
    }
}
