using AutoMapper;
using Habr.Common.DTOs.Posts;
using Habr.DataAccess.Entities;

namespace Habr.Common.Helpers.AutoMapper
{
    public class PostProfile : Profile
    {
        public PostProfile()
        {
            CreateMap<Draft, Post>()
                .ForMember(
                dest => dest.Id,
                opt => opt.Ignore())
                .ForMember(
                dest => dest.Title,
                opt => opt.MapFrom(src => src.Title))
                .ForMember(
                dest => dest.Text,
                opt => opt.MapFrom(src => src.Text))
                .ForMember(
                dest => dest.Created,
                opt => opt.MapFrom(src => src.Created));

            // ReverseMap doesn't work. 
            CreateMap<Post, Draft>()
                .ForMember(
                dest => dest.Id,
                opt => opt.Ignore())
                .ForMember(
                dest => dest.Title,
                opt => opt.MapFrom(src => src.Title))
                .ForMember(
                dest => dest.Text,
                opt => opt.MapFrom(src => src.Text))
                .ForMember(
                dest => dest.Created,
                opt => opt.MapFrom(src => src.Created));

            CreateMap<DraftPostDto, Draft>()
                .ForMember(
                dest => dest.Text,
                opt => opt.MapFrom(src => src.Text))
                .ForMember(
                dest => dest.Title,
                opt => opt.MapFrom(src => src.Title))
                .ForMember(
                dest => dest.UserId,
                opt => opt.MapFrom(src => src.UserId));

            CreateMap<DraftPostDto, Post>()
                .ForMember(
                dest => dest.Text,
                opt => opt.MapFrom(src => src.Text))
                .ForMember(
                dest => dest.Title,
                opt => opt.MapFrom(src => src.Title))
                .ForMember(
                dest => dest.UserId,
                opt => opt.MapFrom(src => src.UserId));

            CreateMap<Draft, DraftDto>()
                .ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => src.Id))
                .ForMember(
                dest => dest.Title,
                opt => opt.MapFrom(src => src.Title))
                .ForMember(
                dest => dest.Text,
                opt => opt.MapFrom(src => src.Text))
                .ForMember(
                dest => dest.Created,
                opt => opt.MapFrom(src => src.Created))
                .ForMember(
                dest => dest.LastUpdated,
                opt => opt.MapFrom(src => src.LastUpdated));

            CreateMap<Post, PostDto>()
                .ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => src.Id))
                .ForMember(
                dest => dest.Title,
                opt => opt.MapFrom(src => src.Title))
                .ForMember(
                dest => dest.Text,
                opt => opt.MapFrom(src => src.Text))
                .ForMember(
                dest => dest.Edited,
                opt => opt.MapFrom(src => src.Edited))
                .ForMember(
                dest => dest.Created,
                opt => opt.MapFrom(src => src.Created));
        }
    }
}
