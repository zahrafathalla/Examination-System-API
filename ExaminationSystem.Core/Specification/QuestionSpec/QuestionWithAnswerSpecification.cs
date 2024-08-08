using ExaminationSystem.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExaminationSystem.Core.Specification.QuestionSpec
{
    public class QuestionWithAnswerSpecification :BaseSpecification<Question>
    {
        public QuestionWithAnswerSpecification()
            :base()
        {
            Includes.Add(q => q.Include(q => q.Choices));
        }
        public QuestionWithAnswerSpecification(List<int> questionIds)
            : base(q => questionIds.Contains(q.Id))
        {
            Includes.Add(q => q.Include(q => q.Choices));
        }

    }
}
