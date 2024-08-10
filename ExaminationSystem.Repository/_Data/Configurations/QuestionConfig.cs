using ExaminationSystem.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace ExaminationSystem.Repository.Data.Configurations
{
    public class QuestionConfig : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.Property(Q => Q.Text).IsRequired();

            builder.Property(Q => Q.QuestionLevel)
                .HasConversion
                (
                level => level.ToString(),
                level => (QuestionLevel) Enum.Parse(typeof(QuestionLevel), level)
                );

        }
    }
}
