using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExaminationSystem.Repository._Identity
{
    public static class ApplicationIdentityContextSeed
    {
        public static async Task SeedRoleAsync (RoleManager<IdentityRole> roleManager)
        {
            var roles = new[] { "student", "instructor" };

            foreach (var role in roles)
            {
                if(! await roleManager.RoleExistsAsync (role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}
