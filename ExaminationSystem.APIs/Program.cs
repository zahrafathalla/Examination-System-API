
using ExaminationSystem.API.Extensions;
using ExaminationSystem.APIs.Extensions;
using ExaminationSystem.APIs.Helper;
using ExaminationSystem.Core;
using ExaminationSystem.Core.Contracts;
using ExaminationSystem.Core.Entities.Identity;
using ExaminationSystem.Core.ServiceContracts;
using ExaminationSystem.Repository;
using ExaminationSystem.Repository._Identity;
using ExaminationSystem.Repository.Data;
using ExaminationSystem.Repository.GenericRepository;
using ExaminationSystem.Service.AuthService;
using ExaminationSystem.Service.CourseService;
using ExaminationSystem.Service.ExamService;
using ExaminationSystem.Service.QuestionService;
using ExaminationSystem.Service.ResultService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace ExaminationSystem.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
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

            builder.Services.AddDbContext<ApplicationIdentityDBContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });

            builder.Services.AddApplicationService();
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(option =>
            {
                option.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JWT:ValidAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:AuthKey"])),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                };
            });

            builder.Services.AddSwaggerDocumentation();

   
            var app = builder.Build();

            #region Update DataBase
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var _roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var _loggerFactory = services.GetRequiredService<ILoggerFactory>();
            var _dbcontext = services.GetRequiredService<ApplicationDbContext>();
            var _identityDBContext = services.GetRequiredService<ApplicationIdentityDBContext>();
            try
            {
                await _dbcontext.Database.MigrateAsync();
                await _identityDBContext.Database.MigrateAsync();

                await ApplicationIdentityContextSeed.SeedRoleAsync(_roleManager);
            }
            catch (Exception ex)
            {

                var logger = _loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "error during apply migrations");
            } 
            #endregion


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
