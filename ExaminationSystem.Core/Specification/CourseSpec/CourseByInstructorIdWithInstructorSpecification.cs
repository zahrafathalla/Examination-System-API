using ExaminationSystem.Core.Entities;
using Microsoft.EntityFrameworkCore;
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
            Includes.Add(c => c.Include(c=>c.Instructor));
            
        }
    }
}
