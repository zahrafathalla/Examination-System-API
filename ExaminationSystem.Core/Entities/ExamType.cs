using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ExaminationSystem.Core.Entities
{
    public enum ExamType
    {
        [EnumMember(Value = "Final")]
        Final,

        [EnumMember(Value = "Quiz")]
        Quiz
    }
}
