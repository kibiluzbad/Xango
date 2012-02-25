using AutoMapper;
using Blog.Domain;
using Blog.UI.Models;

namespace Blog.UI.Infra.AutoMapper
{
    public class CommentToCommentViewModelProfile : Profile
    {
        public CommentToCommentViewModelProfile()
        {
            Mapper.CreateMap<Comment, CommentViewModel>()
                .ForMember(c => c.Date, c => c.MapFrom(d => d.CreatedAt.ToString("dd.MM.yyyy")))
                .ForMember(c => c.Time, c => c.MapFrom(d => d.CreatedAt.ToString("HH:mm")));
        }
    }
}