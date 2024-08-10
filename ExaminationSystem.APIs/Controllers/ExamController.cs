using AutoMapper;
using ExaminationSystem.APIs.Dtos;
using ExaminationSystem.Core.Entities;
using ExaminationSystem.Core.ServiceContracts;
using ExaminationSystem.Service.CourseService;
using ExaminationSystem.Service.ExamService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.APIs.Controllers
{

    public class ExamController : BaseController
    {
        private readonly IExamService _examService;
        private readonly IMapper _mapper;

        public ExamController(
            IExamService examService,
            IMapper mapper)
        {
            _examService = examService;
            _mapper = mapper;
        }
        [Authorize(Roles = "instructor")]
        [HttpPost("Manual")]
        public async Task<ActionResult<ExamToReturnDto>> AddExamManually(ManualExamDto model)
        {

            var addedExam = await _examService.CreateManualExamAsync(model.StartDate, model.ExamType, model.QuestionsIDs, model.CourseId, model.InstructorId);
            if (addedExam is null)
                return BadRequest();

            return Ok(_mapper.Map<ExamToReturnDto>(addedExam));
        }
        [Authorize(Roles = "instructor")]
        [HttpPost("Automatic")]

        public async Task<ActionResult<ExamToReturnDto>> AddExamAutomatically(AutomaticExamDto model)
        {

            var addedExam = await _examService.CreateAutomaticExamAsync(model.StartDate, model.ExamType, model.numberOfQuestions, model.CourseId, model.InstructorId);
            if (addedExam is null)
                return BadRequest();

            return Ok(_mapper.Map<ExamToReturnDto>(addedExam));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ExamToReturnDto>> UpdateExam(int id, ExamDto model)
        {
            var exam = _mapper.Map<Exam>(model);
            var updatedExam = await _examService.UpdateExamAsync(id, exam);
            if (updatedExam is null)
                return BadRequest();
            return Ok(_mapper.Map<ExamToReturnDto>(updatedExam));

        }

        [Authorize(Roles = "instructor")]
        [HttpDelete("{id}")]

        public async Task<ActionResult<bool>> DeleteExam(int id)
        {
            return Ok(await _examService.DeleteExamAsync(id));
        }

        [Authorize(Roles = "instructor")]
        [HttpPost("Assign")]
        public async Task<ActionResult<bool>> AssignExamToStudent(int examId, int studentId)
        {
            var result = await _examService.AssignToExams(examId, studentId);
            if (!result)
                return BadRequest();

            return Ok(result);

        }

        [Authorize(Roles = "instructor")]
        [HttpGet("{instructorId}")]
        public async Task<ActionResult<IEnumerable<ExamToReturnDto>>> GetExamsByInstructorId(int instructorId)
        {
            var exams = await _examService.GetExamsByInstructorIdAsync(instructorId);
            if (exams is null)
                return NotFound();
            return Ok(_mapper.Map<IEnumerable<ExamToReturnDto>>(exams));
        }

        [Authorize(Roles = "student")]
        [HttpGet("TakeExam/{examId}")]

        public async Task<ActionResult<ExamToReturnDto>> TakeExam(int examId, int studentId)
        {
            var exam = await _examService.TakeExamAsync(examId,studentId);
            if (exam == null)
                return NotFound();
            return Ok(_mapper.Map<ExamToReturnDto>(exam));
        }

        [Authorize(Roles = "student")]
        [HttpPost("submit/{examId}")]

        public async Task<ActionResult<ResultToReturnDto>> SubmitExam(int examId, int studentId, List<string> answers)
        {
            var result = await _examService.SubmitExamAsync(examId, studentId,answers);

            if(result == null) return BadRequest();

            return Ok(_mapper.Map<ResultToReturnDto>(result));
        }


    }
}
