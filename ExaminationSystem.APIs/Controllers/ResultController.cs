using AutoMapper;
using ExaminationSystem.APIs.Dtos;
using ExaminationSystem.Core.Entities;
using ExaminationSystem.Core.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.APIs.Controllers
{

    public class ResultController : BaseController
    {
        private readonly IResultService _resultService;
        private readonly IMapper _mapper;

        public ResultController(
            IResultService resultService,
            IMapper mapper)
        {
            _resultService = resultService;
            _mapper = mapper;
        }

        //[HttpGet("{examId}/{studentId}")]

        //public async Task<ActionResult<ResultToReturnDto>> EvaluateExam (int examId, int studentId)
        //{
        //    var result = await _resultService.EvaluateExam(examId, studentId);
        //    if (result == null)
        //        return BadRequest();
        //    return Ok(_mapper.Map<ResultToReturnDto>(result));
        //}

        [HttpGet("{resultId}")]
        public async Task<ActionResult<ResultToReturnDto>> ViewResults(int resultId)
        {
            var result = await _resultService.ViewResults(resultId);
            return Ok(_mapper.Map<ResultToReturnDto>(result));

        }
    }
}
