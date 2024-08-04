﻿using ExaminationSystem.Core.Entities;


namespace ExaminationSystem.Core.ServiceContracts
{
    public interface IExamService
    {
        //Task<Exam> CreateExam(Exam exam, List<int> QuestionsIDs, int numberOfQuestions, bool IsMunuall);

        Task<Exam> CreateManualExamAsync(DateTime startDate, ExamType examType, List<int> questionIds, int courseId, int InstructorId);
        Task<Exam> CreateAutomaticExamAsync(DateTime startDate, ExamType examType, int numberOfQuestions, int courseId, int InstructorId);
        Task<Exam> UpdateExamAsync(int id,Exam exam);
        Task<bool> DeleteExamAsync(int id);
        Task<bool> AssignToExams(int studentId, int examId);
        Task<IEnumerable<Exam>> GetExamsByInstructorIdAsync(int instructorId);
    }
}
