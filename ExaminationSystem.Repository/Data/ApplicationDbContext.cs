using ExaminationSystem.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExaminationSystem.Repository.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            :base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public DbSet<Student> students { get; set; }
        public DbSet<Course>  courses { get; set; }
        public DbSet<Choice> choices { get; set; }
        public DbSet<Exam> exams { get; set; }
        public DbSet<Instructor> instructors { get; set; }
        public DbSet<Question> questions{ get; set; }
        public DbSet<Result> results { get; set; }




    }
}
