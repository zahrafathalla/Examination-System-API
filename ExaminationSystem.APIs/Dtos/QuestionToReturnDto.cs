namespace ExaminationSystem.APIs.Dtos
{
    public class QuestionToReturnDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int Grade { get; set; }
        public string QuestionLevel { get; set; }
        public ICollection<ChoiceDto> Choices { get; set; } = new List<ChoiceDto>();
        

    }
}
