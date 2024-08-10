using ExaminationSystem.Core;
using ExaminationSystem.Core.Entities;
using ExaminationSystem.Core.ServiceContracts;
using ExaminationSystem.Core.Specification.CourseSpec;
using ExaminationSystem.Core.Specification.ExamQuestionSpec;
using ExaminationSystem.Core.Specification.ExamSpec;
using ExaminationSystem.Core.Specification.QuestionSpec;
using ExaminationSystem.Repository;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ExaminationSystem.Service.ExamService
{
    public class ExamService : IExamService
    {
        private readonly IunitOfWork _unitOfWork;
        private readonly IResultService _resultService;

        public ExamService(
            IunitOfWork unitOfWorrk,
            IResultService resultService)
        {
            _unitOfWork = unitOfWorrk;
            _resultService = resultService;
        }

        public async Task<Exam> CreateAutomaticExamAsync(DateTime startDate, ExamType examType, int numberOfQuestions,int courseId, int InstructorId)
        {
            var spec = new QuestionWithAnswerSpecification();
            var existingQuestions = await _unitOfWork.Repository<Question>().GetAllWithSpecificationAsync(spec);
            var questions = GetQuestions(existingQuestions, numberOfQuestions);

            var totalGrade = questions.Sum(q => q.Grade);

            var exam = new Exam()
            {
                StartDate = startDate,
                TotalGrade = totalGrade,
                ExamType = examType,
                CourseId = courseId,
                InstructorId = InstructorId
            };

            await _unitOfWork.Repository<Exam>().AddAsync(exam);

            var result = await _unitOfWork.CompleteAsync();
            if (result <= 0)
                return null;
           
            await AddQuestionsToExam(exam, questions);

            return exam;
        }
        public async Task<Exam> CreateManualExamAsync(DateTime startDate, ExamType examType, List<int> questionIds,int courseId, int InstructorId)
        {
            var spec = new QuestionWithAnswerSpecification(questionIds);
            var questions = await _unitOfWork.Repository<Question>().GetAllWithSpecificationAsync(spec);

            var totalGrade = questions.Sum(q => q.Grade);

            var exam = new Exam()
            {
                StartDate = startDate,
                TotalGrade = totalGrade,
                ExamType = examType,
                CourseId= courseId,
                InstructorId = InstructorId              
            };

            await _unitOfWork.Repository<Exam>().AddAsync(exam);

            var result = await _unitOfWork.CompleteAsync();
            if (result <= 0)
                return null;

            await AddQuestionsToExam(exam, questions);

            return exam;

        }
        public async Task<Exam> UpdateExamAsync(int id, Exam exam)
        {
            var examRepo = _unitOfWork.Repository<Exam>();
            var spec = new ExamWithQuestionAndChoicesSpecification(id);

            var existingExam = await examRepo.GetByIdWithSpecificationAsync(spec);

            if (existingExam == null) return null;

            existingExam.StartDate = exam.StartDate;
            existingExam.ExamType = exam.ExamType;

            examRepo.Update(existingExam);
            var result = await _unitOfWork.CompleteAsync();
            if (result <= 0) return null;

            return existingExam;
        }
        public async Task<bool> DeleteExamAsync(int id)
        {
            var examRepo = _unitOfWork.Repository<Exam>();

            var exam = await examRepo.GetByIdAsync(id);

            if (exam == null) return false;

            examRepo.Delete(exam);
            var result = await _unitOfWork.CompleteAsync();
            return result > 0;
        }
        public async Task<IEnumerable<Exam>> GetExamsByInstructorIdAsync(int instructorId)
        {
            var spec = new ExamByInstructorIdWithInstructorSpecification(instructorId);

            var exams = await _unitOfWork.Repository<Exam>().GetAllWithSpecificationAsync(spec);

            return exams;
        }
        public async Task<bool> AssignToExams(int examId, int studentId)
        {
            var studentExamRepo = _unitOfWork.Repository<StudentExam>();

            var exam = await _unitOfWork.Repository<Exam>().GetByIdAsync(examId);
            if (exam == null) return false;

            //check that Instructor assigned a student to a course first, 
            var studentCourse = await _unitOfWork.Repository<StudentCourse>().GetAsync(sc => sc.StudentId == studentId && sc.CourseId == exam.CourseId);
            if (studentCourse.Count() == 0) return false;

            var studentExam = new StudentExam()
            {
                StudentId = studentId,
                ExamId = examId
            };
            await studentExamRepo.AddAsync(studentExam);
            var result = await _unitOfWork.CompleteAsync();
            return result > 0;

        }
        public async Task<Exam> TakeExamAsync(int examId, int studentId)
        {
            var studentExamRepo = _unitOfWork.Repository<StudentExam>();

            //check that Instructor assigned a student to a Exam first, 
            var studentExam = (await studentExamRepo.GetAsync(se=>se.ExamId== examId && se.StudentId== studentId)).FirstOrDefault();

            if (studentExam == null) return null;

            var spec = new ExamWithQuestionAndChoicesSpecification(examId);
            var exam = await _unitOfWork.Repository<Exam>().GetByIdWithSpecificationAsync(spec);

            if (exam == null) return null;

            //Student can take many quiz exams, but he can take only one final exam
            if (exam.ExamType == ExamType.Final && studentExam.IsSubmitted == true)
            {
                var finalExams = await studentExamRepo.GetAsync(se => se.StudentId == studentId && se.Exam.ExamType == ExamType.Final);
                if (finalExams.Count() > 1) return null;
            }
            return exam;
        }
        public async Task<Result> SubmitExamAsync(int examId, int studentId, List<string> answers)
        {
            var studentExam = (await _unitOfWork.Repository<StudentExam>()
                        .GetAsync(se => se.StudentId == studentId && se.ExamId == examId)).FirstOrDefault();
            if(studentExam == null) return null;

            var spec = new ExamQuestionSpecification(examId);
            var examQuestions = await _unitOfWork.Repository<ExamQuestion>()
                        .GetAllWithSpecificationAsync(spec);

            var newExamQuestions = new List<ExamQuestion>();

            for (int i = 0; i < examQuestions.Count(); i++)
            {
                var examQuestion = examQuestions.ElementAt(i);
                var answer = answers[i];

                if (!examQuestion.Question.Choices.Select(c => c.Text).Contains(answer))
                    return null;

                examQuestion.Answer = answer;
                newExamQuestions.Add(examQuestion);
            }

            foreach (var examQuestion in newExamQuestions)
                _unitOfWork.Repository<ExamQuestion>().Update(examQuestion);

            studentExam.IsSubmitted = true;
            _unitOfWork.Repository<StudentExam>().Update(studentExam);

            var result = await _unitOfWork.CompleteAsync();

            if (result > 0)
               return await _resultService.EvaluateExam(examId, studentId);

            return null;
           
        }


        private async Task AddQuestionsToExam(Exam exam, IEnumerable<Question> questions)
        {
            foreach (var question in questions)
            {
                await _unitOfWork.Repository<ExamQuestion>().AddAsync(new ExamQuestion
                {
                    QuestionID = question.Id,
                    ExamID = exam.Id
                });
            }
            await _unitOfWork.CompleteAsync();
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

            int remainingQuestionsNum = numberOfQuestions - allQuestions.Count;

            List<Question> remainingQuestions = new List<Question>();
            if (remainingQuestionsNum > 0)
            {
                var random = new Random();
                remainingQuestions = questions
                    .Where(q => !allQuestions.Contains(q)) 
                    .OrderBy(q=>random.Next())
                    .Take(remainingQuestionsNum)
                    .ToList();
            }
            allQuestions.AddRange(remainingQuestions);

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
