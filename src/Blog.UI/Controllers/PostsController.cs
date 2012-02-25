using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Blog.Domain;
using Blog.Domain.Queries;
using Blog.UI.Models;
using Xango.Common.String;
using Xango.Data;
using Xango.Data.NHibernate.Filters;
using Xango.Data.Query;
using Xango.Mvc.AjaxAntiForgery;
using Xango.Mvc.Controller;
using Xango.Mvc.Filters;

namespace Blog.UI.Controllers
{
    public class PostsController
        : CrudController<IRepository<Post>, Post, PostViewModel, string>
    {
        public PostsController(IRepository<Post> postRepository)
            : base(postRepository)
        { }

        [HttpPost]
        [NeedsPersistence(Order = 1)]
        public ActionResult Comment(string slug, CommentViewModel viewModel)
        {
            Post post = GetEntityById(slug);
            post.Comment(viewModel.Text, viewModel.Author, viewModel.Mail);

            var result = AutoMapper.Mapper.Map<Post, PostViewModel>(post);

            return PartialView("Post", result);
        }

        [HttpGet]
        [NeedsPersistence(Order = 1)]
        public ActionResult Last()
        {
            var query = Repository.CreateQuery<ILastPosts>();
            
            IEnumerable<Post> posts = query.Execute();
            var result = AutoMapper.Mapper.Map<IEnumerable<Post>, IEnumerable<PostViewModel>>(posts);

            return PartialView(result);
        }

        protected override IPagedQuery<Post> GetSearchQuery()
        {
            return Repository.CreateQuery<IPagedPostSearch>();
        }

        protected override void UpdateModelData(string id, PostViewModel viewModel)
        {
            Post post = GetEntityById(id);

            post.Title = viewModel.Title;
            post.Body = viewModel.Body;
            post.Slug = viewModel.Title.Slugify();
        }

        protected override Post LoadFormViewModel(PostViewModel viewModel)
        {
            return GetEntityById(viewModel.Slug);
        }

        protected override Post GetEntityById(string id)
        {
            var query = Repository.CreateQuery<IFindPostBySlug>();
            query.Slug = id;

            ViewBag.Slug = id;

            return query.Execute();            
        }

        public override void BeforeRepositoryAdd(Post model)
        {
            model.Slug = model.Title.Slugify();
        }
    }
}