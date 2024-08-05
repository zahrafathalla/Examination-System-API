using ExaminationSystem.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExaminationSystem.Core.Specification.ExamSpec
{
    public class ExamSpecification :BaseSpecification<Exam>
    {
        public ExamSpecification(int id)
            :base(e=>e.Id== id)
        {
            Include.Add(e => e.ExamQuestions);
        }
    }
}
