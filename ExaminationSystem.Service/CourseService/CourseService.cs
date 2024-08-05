using ExaminationSystem.Core;
using ExaminationSystem.Core.Entities;
using ExaminationSystem.Core.ServiceContracts;
using ExaminationSystem.Core.Specification.CourseSpec;

namespace ExaminationSystem.Service.CourseService
{
    public class CourseService : ICourseService
    {
        private readonly IunitOfWork _unitOfWork;
        public CourseService(IunitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Course> AddCourseAsync(string name, int creditHours, int instructorId)
        {
            var courseRepo = _unitOfWork.Repository<Course>();
            var course = new Course()
            {
                Name = name,
                CreditHours = creditHours,
                InstructorId = instructorId
            };

            await courseRepo.AddAsync(course);

            var result = await _unitOfWork.CompleteAsync();

            if (result <= 0) return null;

            //var createdCourse = await _unitOfWork.Repository<Course>().GetByIdAsync(course.Id);
            //var instructor = await _unitOfWork.Repository<Instructor>().GetByIdAsync(createdCourse.InstructorId);
            //if (instructor == null) return null;
            //createdCourse.Instructor = instructor;

            //// to return course with Instructor
            var spec = new CourseSpecification(course.Id);
            var createdCourse = await courseRepo.GetByIdWithSpecificationAsync(spec);

            return createdCourse;
        }

        public async Task<Course> EditCourseAsync(int id, Course course)
        {
            var courseRepo = _unitOfWork.Repository<Course>();
            
            var existingCourse = await courseRepo.GetByIdAsync(id);
            if (existingCourse == null) return null;

            existingCourse.Name = course.Name;
            existingCourse.CreditHours = course.CreditHours;
            existingCourse.InstructorId = course.InstructorId;

            //var instructor = await _unitOfWork.Repository<Instructor>().GetByIdAsync(existingCourse.InstructorId);
            //if (instructor == null) return null;
            //existingCourse.Instructor = instructor;

            courseRepo.Update(existingCourse);
            var result = await _unitOfWork.CompleteAsync();

            if (result <= 0) return null;

            ////To Get instructor if instructorId changes
            var spec = new CourseSpecification(id);
            var updatedCourse = await courseRepo.GetByIdWithSpecificationAsync(spec);

            return updatedCourse;
        }

        public async Task<bool> DeleteCourseAsync(int courseId)
        {
            var courseRepo = _unitOfWork.Repository<Course>();

            var course = await courseRepo.GetByIdAsync(courseId);

            if (course == null) return false;

            courseRepo.Delete(course);
            var result = await _unitOfWork.CompleteAsync();
            return result > 0;

        }

        public async Task<bool> EnrollStudentAsync(int studentId, int courseId)
        {
            var studentCourseRepo = _unitOfWork.Repository<StudentCourse>();

            var studentCourse = new StudentCourse
            {
                StudentId = studentId,
                CourseId = courseId
            };

            await studentCourseRepo.AddAsync(studentCourse);
            var result = await _unitOfWork.CompleteAsync();
            return result > 0;
        }

        public async Task<IEnumerable<Course>> GetCoursesByInstructorIdAsync(int instructorId)
        {

            var spec = new CourseByInstructorIdWithInstructorSpecification(instructorId);

            var courses = await _unitOfWork.Repository<Course>().GetAllWithSpecificationAsync(spec);

            return courses;
        }
    
           
        
    }
}
