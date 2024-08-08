using ExaminationSystem.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExaminationSystem.Core.ServiceContracts
{
    public interface IResultService
    {
        Task<Result> EvaluateExam(int examId, int studentId);
        Task<Result> ViewResults(int resultId);

    }
}
