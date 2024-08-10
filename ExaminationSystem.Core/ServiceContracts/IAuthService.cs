using ExaminationSystem.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExaminationSystem.Core.ServiceContracts
{
    public interface IAuthService
    {
        public Task<string> GenerateToken(AppUser user, UserManager<AppUser> userManager);
    }
}
