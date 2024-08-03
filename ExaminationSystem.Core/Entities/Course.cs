using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExaminationSystem.Core.Entities
{
    public class Course :BaseEntity
    {
        public string Name { get; set; }
        public int CreditHours { get; set; }
        public int InstructorId { get; set; }
        public Instructor Instructor { get; set; }        
        public ICollection<Exam> Exams { get; set; } = new HashSet<Exam>();
        public ICollection<StudentCourse> StudentCourses { get; set; } = new HashSet<StudentCourse>();

    }
}
