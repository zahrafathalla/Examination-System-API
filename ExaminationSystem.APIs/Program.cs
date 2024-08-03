
using ExaminationSystem.APIs.Helper;
using ExaminationSystem.Core;
using ExaminationSystem.Core.Contracts;
using ExaminationSystem.Core.ServiceContracts;
using ExaminationSystem.Repository;
using ExaminationSystem.Repository.Data;
using ExaminationSystem.Repository.GenericRepository;
using ExaminationSystem.Service.CourseService;
using ExaminationSystem.Service.ExamService;
using ExaminationSystem.Service.QuestionService;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ExaminationSystem.APIs
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers().AddNewtonsoftJson(option =>
            {
                option.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            }); ;
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
                .LogTo(log => Debug.WriteLine(log), LogLevel.Information)
                .EnableSensitiveDataLogging(); ;
            });

            builder.Services.AddScoped(typeof(IunitOfWork), typeof(UnitOfWork));

            builder.Services.AddScoped(typeof(IGenericRepository<>),typeof(GenericRepository<>));
            builder.Services.AddScoped(typeof(IExamService), typeof(ExamService));
            builder.Services.AddScoped(typeof(IQuestionService), typeof(QuestionService));
            builder.Services.AddScoped(typeof(ICourseService), typeof(CourseService));

            builder.Services.AddAutoMapper(typeof(MappingProfile));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
