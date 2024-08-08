namespace ExaminationSystem.APIs.Dtos
{
    public class QuestionForExamToReturnDto
    {
        public string QuestionsText { get; set; }
        public List<string> ChoicesText { get; set; }
    }
}