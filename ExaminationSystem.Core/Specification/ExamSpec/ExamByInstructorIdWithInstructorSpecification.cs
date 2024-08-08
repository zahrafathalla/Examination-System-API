using ExaminationSystem.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExaminationSystem.Core.Specification.ExamSpec
{
    public class ExamByInstructorIdWithInstructorSpecification :BaseSpecification<Exam>
    {
        public ExamByInstructorIdWithInstructorSpecification(int instructorId)
            :base(e=>e.InstructorId == instructorId)
        {
            Includes.Add(e => e.Include(e => e.ExamQuestions)
                               .ThenInclude(eq => eq.Question)
                               .ThenInclude(q => q.Choices));
        }
    }
}
