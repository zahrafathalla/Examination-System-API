
namespace ExaminationSystem.Core.Entities
{
    public class Choice : BaseEntity
    {
        public string Text { get; set; }
        public bool IsRightAnswer { get; set; }
        public int QuestionID { get; set; }
        public Question Question { get; set; }

    }
}
