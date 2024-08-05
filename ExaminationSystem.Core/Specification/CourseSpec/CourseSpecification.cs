using ExaminationSystem.Core.Entities;

namespace ExaminationSystem.Core.Specification.CourseSpec
{
    public class CourseSpecification : BaseSpecification<Course>
    {
        public CourseSpecification(int id)
            : base(c => c.Id == id)
        {
            Include.Add(c => c.Instructor);
        }


    }
}
