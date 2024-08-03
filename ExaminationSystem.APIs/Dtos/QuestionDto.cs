using ExaminationSystem.Core.Entities;

namespace ExaminationSystem.APIs.Dtos
{
    public class QuestionDto
    {
        public string Text { get; set; }
        public int Grade { get; set; }
        public QuestionLevel QuestionLevel { get; set; }
        public Dictionary<string ,bool> Choices { get; set; } = new Dictionary<string, bool>();
        
    }
}

