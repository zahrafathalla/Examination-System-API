using ExaminationSystem.Core.Entities;

namespace ExaminationSystem.APIs.Dtos
{
    public class ResultToReturnDto
    {
        public int Grade { get; set; }
        public int StudentId { get; set; }
        public int ExamId { get; set; }


    }
}
