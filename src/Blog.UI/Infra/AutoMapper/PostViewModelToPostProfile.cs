using System;
using AutoMapper;
using Blog.Domain;
using Blog.UI.Models;
using Microsoft.Practices.ServiceLocation;
using Xango.Data;

namespace Blog.UI.Infra.AutoMapper
{
    public class PostViewModelToPostProfile : Profile
    {
        public PostViewModelToPostProfile()
        {
            Mapper.CreateMap<PostViewModel, Post>()
                .ForMember(c => c.CreatedAt, c => c.MapFrom(d => DateTime.Now));
        }
    }
}