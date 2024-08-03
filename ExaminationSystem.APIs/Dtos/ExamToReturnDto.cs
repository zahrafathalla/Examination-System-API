using ExaminationSystem.Core.Entities;

namespace ExaminationSystem.APIs.Dtos
{
    public class ExamToReturnDto
    {
        public DateTime StartDate { get; set; }
        public int TotalGrade { get; set; }
        public string ExamType { get; set; }
        public ICollection<ExamQuestion> ExamQuestions { get; set; }
    }
}
