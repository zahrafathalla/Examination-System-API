using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExaminationSystem.Core.Entities
{
    public class Question : BaseEntity
    {
        public string Text { get; set; }
        public int Grade { get; set; }
        public QuestionLevel QuestionLevel { get; set; }
        public ICollection<Choice> Choices { get; set; } = new HashSet<Choice>();
        public ICollection<ExamQuestion> ExamQuestions { get; set; } = new HashSet<ExamQuestion>();

    }
}

