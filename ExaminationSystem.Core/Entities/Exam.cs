using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExaminationSystem.Core.Entities
{
    public class Exam : BaseEntity
    {
        public DateTime StartDate { get; set; } = DateTime.Now;
        public int TotalGrade { get; set; }
        public ExamType ExamType { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public int InstructorId { get; set; }
        public Instructor Instructor { get; set; }
        public ICollection<Result> Results { get; set; } = new HashSet<Result>();
        public ICollection<ExamQuestion> ExamQuestions { get; set; } = new HashSet<ExamQuestion>();

    }
}
