using AutoMapper;
using ChatBotApplication.Dto.Course;
using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatBotApplication.Common.Mapping
{
    public class CourseMappingProfile : Profile
    {
        public CourseMappingProfile()
        {
            // Map từ Request -> Entity
            CreateMap<CreateCourseRequest, Course>();

            CreateMap<UpdateCourseRequest, Course>();

            // Map từ Entity -> Response
            CreateMap<Course, CourseResponse>()
                .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src => src.Subject.Name))
                .ForMember(dest => dest.Status, opt=> opt.MapFrom(src=> src.Status.ToString()))
                .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.Level.ToString()));
        }
    }
}
