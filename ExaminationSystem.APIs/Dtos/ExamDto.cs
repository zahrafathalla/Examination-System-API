using ExaminationSystem.Core.Entities;

namespace ExaminationSystem.APIs.Dtos
{
    public class ExamDto
    {
        public DateTime StartDate { get; set; }
        public ExamType ExamType { get; set; }
    }
}
