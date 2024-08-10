using AutoMapper;
using ExaminationSystem.APIs.Dtos;
using ExaminationSystem.Core.Entities;
using ExaminationSystem.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.APIs.Controllers
{
    [Authorize(Roles = "instructor")]
    public class QuestionController : BaseController
    {
        private readonly IQuestionService _questionService;
        private readonly IMapper _mapper;

        public QuestionController(
            IQuestionService questionService,
            IMapper mapper)
        {
            _questionService = questionService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<QuestionToReturnDto>> AddQuestion (QuestionDto model)
        {
            var addedQuestion =await _questionService.AddQuestion(model.Text,model.Grade,model.QuestionLevel,model.Choices);

            if (addedQuestion is null)
                return BadRequest();

            return Ok(_mapper.Map<QuestionToReturnDto>(addedQuestion));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<QuestionToReturnDto>> EditQuestion(int id, QuestionDto model)
        {
            var question = _mapper.Map<Question>(model);
            var Updatedquestion =await _questionService.EditQuestion(id, question);

            if (Updatedquestion is null)
                return BadRequest();

            return Ok(_mapper.Map<QuestionToReturnDto>(Updatedquestion));

        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteQuestion(int id)
        {
            return Ok(await _questionService.DeleteQuestion(id));
        }

    }
}
