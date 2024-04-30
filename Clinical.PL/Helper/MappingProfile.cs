using AutoMapper;
using Clinical.DAL.Entities;
using Clinical.PL.Models;

namespace Clinical.PL.Mapper
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
           CreateMap<Doctor,DoctorViewModel>().ForMember(dest=>dest.Name,option=>option.MapFrom(src=>src.User.UserName));    
           CreateMap<Doctor,DoctorDetailsViewModel>().ForMember(dest=>dest.Name,option=>option.MapFrom(src=>src.User.UserName))
                .ForMember(dest=>dest.Email,option=>option.MapFrom(src=>src.User.Email));    
           CreateMap<Patient,PatientViewModel>().ForMember(dest => dest.Username, option => option.MapFrom(src => src.User.UserName))
				.ForMember(dest => dest.Email, option => option.MapFrom(src => src.User.Email)); ;
           CreateMap<ReservationViewModel, Appointment>();
           CreateMap<Appointment,PatientProfileViewModel>()
                .ForMember(dest => dest.DoctorEmail, option => option.MapFrom(src => src.Doctor.User.Email))
                .ForMember(dest => dest.DoctorName, option => option.MapFrom(src => src.Doctor.User.UserName))
                .ForMember(dest => dest.DoctorSpecialty, option => option.MapFrom(src => src.Doctor.Specialty));
            CreateMap<Appointment, DoctorProfileViewModel>()
                 .ForMember(dest => dest.patientName, option => option.MapFrom(src => src.Patient.User.UserName))
                 .ForMember(dest => dest.PatientEmail, option => option.MapFrom(src => src.Patient.User.Email))
                 .ForMember(dest => dest.Address, option => option.MapFrom(src => src.Patient.Address));
            CreateMap<DoctorProfileViewModel, Appointment>();
        }
    }
}
