using ExaminationSystem.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExaminationSystem.Core.ServiceContracts
{
    public interface IQuestionService
    {
        Task<Question> AddQuestion(string Text, int Grade, QuestionLevel QuestionLevel, Dictionary<string, bool> choices);
        Task<Question> EditQuestion(int id , Question question);
        Task<bool> DeleteQuestion(int id);

    }
}
