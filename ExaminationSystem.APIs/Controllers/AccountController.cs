using AutoMapper;
using ExaminationSystem.APIs.Dtos;
using ExaminationSystem.Core;
using ExaminationSystem.Core.Entities;
using ExaminationSystem.Core.Entities.Identity;
using ExaminationSystem.Core.ServiceContracts;
using ExaminationSystem.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ExaminationSystem.APIs.Controllers
{

    public class AccountController : BaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IunitOfWork _unitOfWork;
        private readonly IAuthService _authService;

        public AccountController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IunitOfWork unitOfWork,
            IAuthService authService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
                return Unauthorized("Login NotValid");

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password,false);

            if (!result.Succeeded)
                return Unauthorized("Login NotValid");

            return Ok(new UserDto()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Token = await _authService.GenerateToken(user, _userManager)
            }) ;
        }


        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            var user = new AppUser()
            {
                Email = model.Email,
                UserName = model.Email.Split("@")[0],
                FirstName = model.FirstName,
                LastName = model.LastName,
                Birthdate = model.BirthDate
            };
            var result = await _userManager.CreateAsync(user, model.Password);

            if(result.Succeeded)
            {
                if (model.Role == "Student".ToLower())
                {
                    var student = new Student
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                    };
                    await _unitOfWork.Repository<Student>().AddAsync(student);
                }

                else if (model.Role == "Instructor".ToLower())
                {
                    var instructor = new Instructor
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Birthdate = user.Birthdate,
                        EnrollmentDate = DateTime.Now
                    };
                    await _unitOfWork.Repository<Instructor>().AddAsync(instructor);
                }
                await _unitOfWork.CompleteAsync();
                await _userManager.AddToRoleAsync(user,model.Role);
            }
            return Ok(new UserDto()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Token = await _authService.GenerateToken(user, _userManager)
            });
        }
    }
}
