using AlgorizaProject.DAL.Entities;
using AlgorizaProject.Dtos;
using AutoMapper;

namespace AlgorizaProject.Helper
{
    public class MapperProfiler : Profile
    {
        public MapperProfiler()
        {
            CreateMap<Request, BookingDto>()
                .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Patient.FirstName))
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Patient.imageUrl))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.Patient.DateOfBirth))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Patient.Gender))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Patient.PhoneNumber))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Patient.Email)).ReverseMap();
        }
    }
}
