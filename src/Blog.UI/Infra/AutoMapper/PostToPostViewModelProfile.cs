using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AutoMapper;
using Blog.Domain;
using Blog.UI.Models;

namespace Blog.UI.Infra.AutoMapper
{
    public class PostToPostViewModelProfile : Profile
    {
        public PostToPostViewModelProfile()
        {
            Mapper.CreateMap<Post, PostViewModel>()
                .ForMember(c => c.CreatedAt, c => c.MapFrom(d => d.CreatedAt.ToString("dd.MM.yyyy")))
                .ForMember(c => c.Day, c => c.MapFrom(d => d.CreatedAt.ToString("dd")))
                .ForMember(c => c.Month,
                           c =>
                           c.MapFrom(
                               d => CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(d.CreatedAt.Month)))
                .ForMember(c => c.Year, c => c.MapFrom(d => d.CreatedAt.Year))
                .ForMember(c => c.Comments, c => c.MapFrom(
                    d => d.Comments.Select(Mapper.Map<Comment, CommentViewModel>).ToList()));

        }
    }
}