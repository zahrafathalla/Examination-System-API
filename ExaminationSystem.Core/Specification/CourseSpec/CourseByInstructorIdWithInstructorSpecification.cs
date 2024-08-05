using ExaminationSystem.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExaminationSystem.Core.Specification.CourseSpec
{
    public class CourseByInstructorIdWithInstructorSpecification : BaseSpecification<Course>
    {
        public CourseByInstructorIdWithInstructorSpecification(int instructorId)
        : base(c => c.InstructorId == instructorId)
        {
            Include.Add(c => c.Instructor);
        }
    }
}
