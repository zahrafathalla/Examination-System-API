using ExaminationSystem.Core;
using ExaminationSystem.Core.Entities;
using ExaminationSystem.Core.ServiceContracts;
using ExaminationSystem.Repository;

namespace ExaminationSystem.Service.ExamService
{
    public class ExamService : IExamService
    {
        private readonly IunitOfWork _unitOfWork;

        public ExamService(IunitOfWork unitOfWorrk)
        {
            _unitOfWork = unitOfWorrk;
        }

        public async Task<Exam> CreateAutomaticExamAsync(DateTime startDate, ExamType examType, int numberOfQuestions,int courseId, int InstructorId)
        {

            var existingQuestions = await _unitOfWork.Repository<Question>().GetAllAsync();
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
            var questions = await _unitOfWork.Repository<Question>().Get(q => questionIds.Contains(q.Id));

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
            var existingExam = await examRepo.GetByIdAsync(id);

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
        public Task<IEnumerable<Exam>> GetExamsByInstructorIdAsync(int instructorId)
        {
            var exams = _unitOfWork.Repository<Exam>().Get(e=>e.InstructorId==instructorId);
            return exams;
        }


        public async Task<bool> AssignToExams(int studentId, int examId)
        {
            var studentExamRepo = _unitOfWork.Repository<StudentExam>();

            var exam = await _unitOfWork.Repository<Exam>().GetByIdAsync(examId);
            if (exam == null) return false;

            //check that Instructor assigned a student to a course first, 
            var studentCourse = await _unitOfWork.Repository<StudentCourse>().Get(sc => sc.StudentId == studentId && sc.CourseId == exam.CourseId);
            if (studentCourse.Count() == 0) return false;


            //Student can take many quiz exams, but he can take only one final exam
            if (exam.ExamType == ExamType.Final)
            {
                var finalExams = await studentExamRepo.Get(se => se.StudentId == studentId && se.Exam.ExamType == ExamType.Final);
                if(finalExams.Count() > 1 ) return false;
            }

            var studentExam = new StudentExam()
            {
                StudentId = studentId,
                ExamId = examId
            };
            await studentExamRepo.AddAsync(studentExam);
            var result = await _unitOfWork.CompleteAsync();
            return result > 0;

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
