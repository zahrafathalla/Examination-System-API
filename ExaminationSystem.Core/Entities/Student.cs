using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExaminationSystem.Core.Entities
{
    public class Student: BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<StudentExam> StudentExams { get; set; } = new List<StudentExam>();
        public ICollection<Result> ExamResults { get; set; } = new HashSet<Result>();
        public ICollection<StudentCourse> StudentCourses { get; set; } = new HashSet<StudentCourse>();

    }
}
