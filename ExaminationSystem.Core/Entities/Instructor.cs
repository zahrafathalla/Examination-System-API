﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExaminationSystem.Core.Entities
{
    public class Instructor: BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthdate { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public ICollection<Exam> Exams { get; set; } = new HashSet<Exam>();

    }
}
