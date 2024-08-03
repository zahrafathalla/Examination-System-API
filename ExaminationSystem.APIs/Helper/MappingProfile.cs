using AutoMapper;
using ExaminationSystem.APIs.Dtos;
using ExaminationSystem.Core.Entities;
using ExaminationSystem.Service.ExamService;

namespace ExaminationSystem.APIs.Helper
{
    public class MappingProfile :Profile
    {
        public MappingProfile()
        {

            CreateMap<Question, QuestionToReturnDto>();

            CreateMap<QuestionDto, Question>().ForMember(dest => dest.Choices, opt => opt.MapFrom(src => src.Choices.Select(c => new Choice
            {
                Text = c.Key,
                IsRightAnswer = c.Value
            })));

            CreateMap<Choice, ChoiceDto>();

            CreateMap<Exam, ExamToReturnDto>()
                .ForMember(d=> d.ExamQuestions, opt=> opt.MapFrom(s=>s.ExamQuestions));

            CreateMap<Course, CourseToReturnDto>();
            CreateMap<CourseDto, Course>();

            CreateMap<Instructor, InstructorDto>()
                .ForMember(d=>d.FullName ,opt=>opt.MapFrom(s=> $"{s.FirstName} {s.LastName}"));


        }
    }
}
