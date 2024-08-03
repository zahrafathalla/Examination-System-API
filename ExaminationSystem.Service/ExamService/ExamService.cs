using ExaminationSystem.Core;
using ExaminationSystem.Core.Entities;
using ExaminationSystem.Core.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExaminationSystem.Service.ExamService
{
    public class ExamService : IExamService
    {
        private readonly IunitOfWork _unitOfWorrk;

        public ExamService(IunitOfWork unitOfWorrk)
        {
            _unitOfWorrk = unitOfWorrk;
        }

        public async Task<Exam> CreateAutomaticExamAsync(DateTime startDate, int totalGrade, ExamType examType, int numberOfQuestions,int courseId, int InstructorId)
        {
            var exam = new Exam()
            {
                StartDate = startDate,
                TotalGrade = totalGrade,
                ExamType = examType,
                CourseId = courseId,
                InstructorId = InstructorId
            };

            await _unitOfWorrk.Repository<Exam>().AddAsync(exam);

            var result = await _unitOfWorrk.CompleteAsync();
            if (result <= 0)
                return null;

            var existingQuestions = await _unitOfWorrk.Repository<Question>().GetAllAsync();

            var questions = GetQuestions(existingQuestions, numberOfQuestions);

            await AddQuestionsToExam(exam, questions);

            return exam;
        }
        public async Task<Exam> CreateManualExamAsync(DateTime startDate, int totalGrade, ExamType examType, List<int> questionIds,int courseId, int InstructorId)
        {

            var exam = new Exam()
            {
                StartDate = startDate,
                TotalGrade = totalGrade,
                ExamType = examType,
                CourseId= courseId,
                InstructorId = InstructorId
                
            };

            await _unitOfWorrk.Repository<Exam>().AddAsync(exam);

            var result = await _unitOfWorrk.CompleteAsync();
            if (result <= 0)
                return null;

            var questions = await _unitOfWorrk.Repository<Question>().GetAllAsync();

            await AddQuestionsToExam(exam, questions);

            return exam;

        }
        private async Task AddQuestionsToExam(Exam exam, IEnumerable<Question> questions)
        {
            foreach (var question in questions)
            {
                await _unitOfWorrk.Repository<ExamQuestion>().AddAsync(new ExamQuestion
                {
                    QuestionID = question.Id,
                    ExamID = exam.Id
                });
            }
        }
        private IEnumerable<Question> GetQuestions(IEnumerable<Question> questions, int numberOfQuestions)
        {
            int simpleQuestions = numberOfQuestions / 3;
            int mediumQuestions = numberOfQuestions / 3;
            int hardQuestions = numberOfQuestions - simpleQuestions - mediumQuestions;

            var allQuestions = new List<Question>();
            allQuestions.AddRange(GetRandomQuestions(questions, QuestionLevel.Simple, simpleQuestions));
            allQuestions.AddRange(GetRandomQuestions(questions, QuestionLevel.Medium, mediumQuestions));
            allQuestions.AddRange(GetRandomQuestions(questions, QuestionLevel.Hard, hardQuestions));

            return allQuestions;
        }
        private IEnumerable<Question> GetRandomQuestions(IEnumerable<Question> questions, QuestionLevel level, int numberOfQuestions)
        {
            var random = new Random();
            var selectedQuestions =  questions
                .Where(q => q.QuestionLevel == level)
                .OrderBy(q => random.Next())
                .Take(numberOfQuestions)
                .ToList();

            return selectedQuestions;
        }
    }
}
