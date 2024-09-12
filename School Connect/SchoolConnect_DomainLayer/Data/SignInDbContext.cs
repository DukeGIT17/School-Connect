using Microsoft.EntityFrameworkCore;

namespace SchoolConnect_DomainLayer.Data
{
    public class SignInDbContext : DbContext
    {
        public SignInDbContext(DbContextOptions<SignInDbContext> options) : base(options) { }
    }
}
