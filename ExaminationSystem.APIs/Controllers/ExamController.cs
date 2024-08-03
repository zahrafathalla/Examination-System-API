using AutoMapper;
using ExaminationSystem.APIs.Dtos;
using ExaminationSystem.Core.Entities;
using ExaminationSystem.Core.ServiceContracts;
using ExaminationSystem.Service.ExamService;
using Microsoft.AspNetCore.Http;
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

        [HttpPost("Manual")]
        public async Task<ActionResult<ExamToReturnDto>> AddExamManually(ManualExamDto model)
        {

            var addedExam = await _examService.CreateManualExamAsync(model.StartDate, model.TotalGrade, model.ExamType, model.QuestionsIDs, model.CourseId, model.InstructorId); ;
            if (addedExam is null)
                return BadRequest();

            return Ok(_mapper.Map<ExamToReturnDto>(addedExam));
        }
        [HttpPost("Automatic")]
        public async Task<ActionResult<ExamToReturnDto>> AddExamAutomatically(AutomaticExamDto model)
        {

            var addedExam = await _examService.CreateAutomaticExamAsync(model.StartDate, model.TotalGrade, model.ExamType, model.numberOfQuestions, model.CourseId, model.InstructorId); ;
            if (addedExam is null)
                return BadRequest();

            return Ok(_mapper.Map<ExamToReturnDto>(addedExam));
        }
    }
}
