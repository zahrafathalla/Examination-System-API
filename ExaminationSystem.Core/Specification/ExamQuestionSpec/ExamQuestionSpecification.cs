using ExaminationSystem.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExaminationSystem.Core.Specification.ExamQuestionSpec
{
    public class ExamQuestionSpecification :BaseSpecification<ExamQuestion>
    {
        public ExamQuestionSpecification(int examId)
            :base(eq=>eq.ExamID == examId)
        {
            Includes.Add(eq => eq.Include(eq=> eq.Question).ThenInclude(q=>q.Choices));
        }
    }
}
