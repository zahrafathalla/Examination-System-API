using ExaminationSystem.Core.Entities;

namespace ExaminationSystem.APIs.Dtos
{
    public class ExamToReturnDto
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public int TotalGrade { get; set; }
        public string ExamType { get; set; }
        public List<int> QuestionIds { get; set; }
    }
}
