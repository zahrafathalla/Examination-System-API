using ExaminationSystem.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExaminationSystem.Repository.Data.Configurations
{
    public class ExamConfig : IEntityTypeConfiguration<Exam>
    {
        public void Configure(EntityTypeBuilder<Exam> builder)
        {

            builder.Property(Q => Q.ExamType)
                .HasConversion
                (
                level => level.ToString(),
                level => (ExamType)Enum.Parse(typeof(ExamType), level)
                );


        }
    }
}
