using ExaminationSystem.Core;
using ExaminationSystem.Core.Entities;
using ExaminationSystem.Core.ServiceContracts;
using ExaminationSystem.Core.Specification.ExamQuestionSpec;

namespace ExaminationSystem.Service.ResultService
{
    public class ResultService : IResultService
    {
        private readonly IunitOfWork _unitOfWork;
        public ResultService(IunitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result> EvaluateExam(int examId, int studentId)
        {
            var spec = new ExamQuestionSpecification(examId);
            var examQuestions = await _unitOfWork.Repository<ExamQuestion>()
                                          .GetAllWithSpecificationAsync(spec);

            var grade = 0;

            foreach(var examQuestion in examQuestions)
            {
                var studentAnswer = examQuestion.Answer;

                var rightChoice = examQuestion.Question.Choices.FirstOrDefault(c=>c.IsRightAnswer);

                if (rightChoice.Text == studentAnswer)
                    grade += examQuestion.Question.Grade;

            }

            var result = new Result
            {
                ExamId = examId,
                StudentId = studentId,
                Grade = grade
            };

            await _unitOfWork.Repository<Result>().AddAsync(result);
            await _unitOfWork.CompleteAsync();

            return result;

        }

        public async Task<Result> ViewResults(int resultId)
        {
            var result = await _unitOfWork.Repository<Result>().GetByIdAsync(resultId);

            if (result == null)
                return null;

            return result;

        }
    }
}
