using AutoMapper;
using Habr.Common.DTOs.Comments;
using Habr.DataAccess.Entities;

namespace Habr.Common.Helpers.AutoMapper
{
    public class CommentProfile : Profile
    {
        public CommentProfile()
        {
            CreateMap<AddCommentDto, Comment>()
                .ForMember(
                dest => dest.Text,
                opt => opt.MapFrom(src => src.Text))
                .ForMember(
                dest => dest.UserId,
                opt => opt.MapFrom(src => src.UserId))
                .ForMember(
                dest => dest.PostId,
                opt => opt.MapFrom(src => src.PostId));
                
            CreateMap<ReplyCommentDto, Comment>()
                .ForMember(
                dest => dest.Text,
                opt => opt.MapFrom(src => src.Text))
                .ForMember(
                dest => dest.UserId,
                opt => opt.MapFrom(src => src.UserId))
                .ForMember(
                dest => dest.PostId,
                opt => opt.MapFrom(src => src.PostId))
                .ForMember(
                dest => dest.ParentCommentId,
                opt => opt.MapFrom(src => src.ParentCommentId));

            CreateMap<Comment, CommentDto>()
                .ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => src.Id))
                .ForMember(
                dest => dest.Text,
                opt => opt.MapFrom(src => src.Text))
                .ForMember(
                dest => dest.CreateDate,
                opt => opt.MapFrom(src => src.CreateDate))
                .ForMember(
                dest => dest.UserId,
                opt => opt.MapFrom(src => src.UserId))
                .ForMember(
                dest => dest.PostId,
                opt => opt.MapFrom(src => src.PostId))
                .ForMember(
                dest => dest.ParentCommentId,
                opt => opt.MapFrom(src => src.ParentCommentId));
        }
    }
}
