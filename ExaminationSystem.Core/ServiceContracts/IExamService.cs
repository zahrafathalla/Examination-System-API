using ExaminationSystem.Core.Entities;


namespace ExaminationSystem.Core.ServiceContracts
{
    public interface IExamService
    {
        //Task<Exam> CreateExam(Exam exam, List<int> QuestionsIDs, int numberOfQuestions, bool IsMunuall);

        Task<Exam> CreateManualExamAsync(DateTime startDate, int totalGrade, ExamType examType, List<int> questionIds, int courseId, int InstructorId);
        Task<Exam> CreateAutomaticExamAsync(DateTime startDate, int totalGrade, ExamType examType, int numberOfQuestions, int courseId, int InstructorId);
    }
}
