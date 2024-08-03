using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ExaminationSystem.Core.Entities
{
    public enum QuestionLevel
    {
        [EnumMember(Value ="Simple")]
        Simple,

        [EnumMember(Value = "Medium")]
        Medium,

        [EnumMember(Value = "Hard")]
        Hard
    }
}
