using ExaminationSystem.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Core.Specification.CourseSpec
{
    public class CourseSpecification : BaseSpecification<Course>
    {
        public CourseSpecification(int id)
            : base(c => c.Id == id)
        {
            Includes.Add(c => c.Include(c=>c.Instructor));
        }


    }
}
