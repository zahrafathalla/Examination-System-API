using ExaminationSystem.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExaminationSystem.Service.ExamService
{
    public class AutomaticExamDto
    {
        public DateTime StartDate { get; set; }
        public ExamType ExamType { get; set; }
        public int numberOfQuestions { get; set; }
        public int CourseId { get; set; }
        public int InstructorId { get; set; }
    }
}
