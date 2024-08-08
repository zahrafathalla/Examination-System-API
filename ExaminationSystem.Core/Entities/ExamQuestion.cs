using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExaminationSystem.Core.Entities
{
    public class ExamQuestion: BaseEntity
    {
        public int ExamID { get; set; }
        public Exam Exam { get; set; } 
        public int QuestionID { get; set; }
        public Question Question { get; set; }
        public string? Answer { get; set; }

    }
}
