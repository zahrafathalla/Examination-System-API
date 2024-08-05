using ExaminationSystem.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExaminationSystem.Core.ServiceContracts
{
    public interface ICourseService
    {
        Task<Course> AddCourseAsync(string name, int creditHours, int instructorId);
        Task<Course> EditCourseAsync(int id , Course course);
        Task<bool> DeleteCourseAsync(int courseId);
        Task<bool> EnrollStudentAsync(int studentId, int courseId);
        Task<IEnumerable<Course>> GetCoursesByInstructorIdAsync(int instructorId);
    }
}
