using ExaminationSystem.Core.Entities;

namespace ExaminationSystem.APIs.Dtos
{
    public class CourseToReturnDto
    {
        public string Name { get; set; }
        public int CreditHours { get; set; }
        public int InstructorId { get; set; }
        public InstructorDto Instructor { get; set; }
    }
}
