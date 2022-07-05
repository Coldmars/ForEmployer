using AutoMapper;
using Habr.Common.DTOs.User;
using Habr.DataAccess.Entities;

namespace Habr.Common.Helpers.AutoMapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegistrationUserDto, User>()
                .ForMember(
                dest => dest.Name,
                opt => opt.MapFrom(src => src.Name))
                .ForMember(
                dest => dest.Email,
                opt => opt.MapFrom(src => src.Email));

            CreateMap<User, UserWithTokenDto>()
                .ForMember(
                dest => dest.Name,
                opt => opt.MapFrom(src => src.Name))
                .ForMember(
                dest => dest.Email,
                opt => opt.MapFrom(src => src.Email));

            CreateMap<User, UserDto>()
                .ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => src.Id))
                .ForMember(
                dest => dest.Name,
                opt => opt.MapFrom(src => src.Name))
                .ForMember(
                dest => dest.Email,
                opt => opt.MapFrom(src => src.Email));
        }
    }
}
