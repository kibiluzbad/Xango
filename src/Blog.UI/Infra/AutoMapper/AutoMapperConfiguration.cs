using AutoMapper;
using Microsoft.Practices.ServiceLocation;

namespace Blog.UI.Infra.AutoMapper
{
    public static class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.AddProfile(new PostToPostViewModelProfile());
            Mapper.AddProfile(new PostViewModelToPostProfile());
            Mapper.AddProfile(new CommentToCommentViewModelProfile());
        }
    }
}