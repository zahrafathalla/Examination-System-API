using ExaminationSystem.Core;
using ExaminationSystem.Core.Entities;
using ExaminationSystem.Core.ServiceContracts;
using ExaminationSystem.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExaminationSystem.Service.QuestionService
{
    public class QuestionService : IQuestionService
    {
        private readonly IunitOfWork _unitOfWork;

        public QuestionService(IunitOfWork unitOfWorrk)
        {
            _unitOfWork = unitOfWorrk;
        }
        public async Task<Question> AddQuestion(string text, int grade, QuestionLevel questionLevel, Dictionary<string, bool> choices)
        {
            var question = new Question()
            { 
                Text = text,
                Grade = grade,
                QuestionLevel = questionLevel,   
                
            };


            foreach (var choice in choices)
            {
                question.Choices.Add(new Choice
                {
                    Text = choice.Key,
                    IsRightAnswer = choice.Value
                });
            }

            await _unitOfWork.Repository<Question>().AddAsync(question);
            var result = await _unitOfWork.CompleteAsync();

            if (result <= 0)
                return null;

            return question;


        }

        public async Task<Question> EditQuestion(int id, Question question)
        {
            var questionRepo = _unitOfWork.Repository<Question>();
            var existingQuestion = await questionRepo.GetByIdAsync(id);

            if (existingQuestion == null) return null;

            existingQuestion.Text = question.Text;
            existingQuestion.QuestionLevel = question.QuestionLevel;
            existingQuestion.Grade = question.Grade;
            existingQuestion.Choices = question.Choices;

            questionRepo.Update(existingQuestion);
            var result = await _unitOfWork.CompleteAsync();

            if (result <= 0)
                return null;

            return existingQuestion;

        }
        public async Task<bool> DeleteQuestion(int id)
        {
            var questionRepo = _unitOfWork.Repository<Question>();

            var question = await questionRepo.GetByIdAsync(id);

            if (question == null) return false;

            var choiceRepo = _unitOfWork.Repository<Choice>();
            var choices = await choiceRepo.GetAsync(c => c.QuestionID == id);

            foreach (var choice in choices)
                choiceRepo.Delete(choice);
    
            questionRepo.Delete(question);

            var result = await _unitOfWork.CompleteAsync();
            return result > 0;
        }
    }
}
