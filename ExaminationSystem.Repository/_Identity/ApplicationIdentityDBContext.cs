using ExaminationSystem.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Repository._Identity
{
    public class ApplicationIdentityDBContext : IdentityDbContext<AppUser>
    {
        public ApplicationIdentityDBContext(DbContextOptions<ApplicationIdentityDBContext> options)
            :base(options) 
        {
            
        }
    }
}
