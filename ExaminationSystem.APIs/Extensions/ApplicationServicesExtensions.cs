using ExaminationSystem.APIs.Helper;
using ExaminationSystem.Core.Contracts;
using ExaminationSystem.Core.Entities.Identity;
using ExaminationSystem.Core.ServiceContracts;
using ExaminationSystem.Core;
using ExaminationSystem.Repository._Identity;
using ExaminationSystem.Repository.GenericRepository;
using ExaminationSystem.Repository;
using ExaminationSystem.Service.AuthService;
using ExaminationSystem.Service.CourseService;
using ExaminationSystem.Service.ExamService;
using ExaminationSystem.Service.QuestionService;
using ExaminationSystem.Service.ResultService;
using Microsoft.AspNetCore.Identity;

namespace ExaminationSystem.APIs.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            services.AddScoped(typeof(IunitOfWork), typeof(UnitOfWork));
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped(typeof(IExamService), typeof(ExamService));
            services.AddScoped(typeof(IQuestionService), typeof(QuestionService));
            services.AddScoped(typeof(ICourseService), typeof(CourseService));
            services.AddScoped(typeof(IResultService), typeof(ResultService));
            services.AddScoped(typeof(IAuthService), typeof(AuthService));

            services.AddIdentity<AppUser, IdentityRole>()
                        .AddEntityFrameworkStores<ApplicationIdentityDBContext>();

            services.AddAutoMapper(typeof(MappingProfile));

            return services;
        }
    }
}
