using AutoMapper;
using ExaminationSystem.APIs.Dtos;
using ExaminationSystem.Core.Entities;
using ExaminationSystem.Core.ServiceContracts;
using ExaminationSystem.Service.ExamService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.APIs.Controllers
{

    public class CourseController : BaseController
    {
        private readonly ICourseService _courseService;
        private readonly IMapper _mapper;

        public CourseController(ICourseService courseService, IMapper mapper)
        {
            _courseService = courseService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<CourseToReturnDto>> AddCourse(CourseDto model)
        {
            var course = await _courseService.AddCourseAsync(model.Name, model.CreditHours, model.InstructorId);

            if (course is null)
                return BadRequest();

            return Ok(_mapper.Map<CourseToReturnDto>(course));
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<CourseToReturnDto>> EditCourse(int id, CourseDto model)
        {
            var course = _mapper.Map<Course>(model);

            var updatedCourse = await _courseService.EditCourseAsync(id, course);
            if (updatedCourse is null)
                return BadRequest();

            return Ok(_mapper.Map<CourseToReturnDto>(updatedCourse));

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteCourse(int id)
        {
            return Ok(await _courseService.DeleteCourseAsync(id));
        }

        [HttpPost("student")]
        public async Task<ActionResult<bool>> EnrollStudent(int studentId, int courseId)
        {
            var result = await _courseService.EnrollStudentAsync(studentId, courseId);
            if(!result)
                return BadRequest();

            return Ok(result);
        }

        [HttpGet("{instructorId}")]
        public async Task<ActionResult<IEnumerable<CourseToReturnDto>>> GetCoursesByInstructorId(int instructorId)
        {
            var courses = await _courseService.GetCoursesByInstructorIdAsync(instructorId);
            return Ok(_mapper.Map<IEnumerable<CourseToReturnDto>>(courses));
        }

    }
}
