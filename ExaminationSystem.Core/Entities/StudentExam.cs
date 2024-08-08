namespace ExaminationSystem.Core.Entities
{
    public class StudentExam :BaseEntity
    {
        public int StudentId { get; set; }
        public Student Student { get; set; }
        public int ExamId { get; set; }
        public Exam Exam { get; set; }
        public bool IsSubmitted { get; set; }
    }
}