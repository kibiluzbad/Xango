using System;
using System.Collections.Generic;
using System.Linq;
using Blog.Domain;
using Blog.Domain.Queries;
using Blog.Repository.Queries;
using Microsoft.Practices.ServiceLocation;
using Moq;
using NUnit.Framework;
using Xango.Common.String;
using Xango.Data;
using Xango.Data.NHibernate;
using Xango.Data.NHibernate.Queries;

namespace Blog.Tests
{
    [TestFixture]
    public class AdvancedPostSearchTests : NHibernateBaseFixture
    {
        private void LoadPosts(IRepository<Post> repository)
        {
            for (int i = 0; i < 10; i++)
            {
                var post = new Post()
                               {
                                   Body = "Body" + i,
                                   Title = "Title" + i,
                                   Slug = ("Title" + i).Slugify(),
                                   Author = i <= 4 ? "Renata Fan" : "Flavia Alessandra"
                               };

                repository.Add(post);
            }
            Session.Flush();
        }

        [Test]
        public void Can_Search_By_Author_Name()
        {
            var mockServiceLocator = new Mock<IServiceLocator>();

            mockServiceLocator
                .Setup(c => c.GetInstance<IAdvancedPostSearch>())
                .Returns(new AdvancedPostSearch(SessionFactory))
                .Verifiable();

            IServiceLocator serviceLocator = mockServiceLocator.Object;

            var repository = new NHibernateRepository<Post>(SessionFactory, new QueryFactory(serviceLocator));

            LoadPosts(repository);

            var query = repository.CreateQuery<IAdvancedPostSearch>();
            query.AuthorName = "Renata Fan";

            IEnumerable<Post> posts = query.Execute();

            Assert.AreEqual(5, posts.Count());

            mockServiceLocator.VerifyAll();
        }

        [Test]
        public void Can_Search_By_Post_Title()
        {
            var mockServiceLocator = new Mock<IServiceLocator>();

            mockServiceLocator
                .Setup(c => c.GetInstance<IAdvancedPostSearch>())
                .Returns(new AdvancedPostSearch(SessionFactory))
                .Verifiable();

            IServiceLocator serviceLocator = mockServiceLocator.Object;

            var repository = new NHibernateRepository<Post>(SessionFactory, new QueryFactory(serviceLocator));

            LoadPosts(repository);

            var query = repository.CreateQuery<IAdvancedPostSearch>();
            query.Title = "Title1";

            IEnumerable<Post> posts = query.Execute();

            Assert.AreEqual(1, posts.Count());

            mockServiceLocator.VerifyAll();
        }

        [Test]
        public void Can_Search_Posts_By_All_Filters()
        {
            var mockServiceLocator = new Mock<IServiceLocator>();

            mockServiceLocator
                .Setup(c => c.GetInstance<IAdvancedPostSearch>())
                .Returns(new AdvancedPostSearch(SessionFactory))
                .Verifiable();

            IServiceLocator serviceLocator = mockServiceLocator.Object;

            var repository = new NHibernateRepository<Post>(SessionFactory, new QueryFactory(serviceLocator));

            LoadPosts(repository);

            var query = repository.CreateQuery<IAdvancedPostSearch>();
            query.AuthorName = "Flavia Alessandra";
            query.Title = "Title9";
            query.CreatedBetween = new Period(DateTime.Today.AddDays(-1), DateTime.Today.AddDays(1));

            IEnumerable<Post> posts = query.Execute();

            Assert.AreEqual(1, posts.Count());

            mockServiceLocator.VerifyAll();
        }

        [Test]
        public void Can_Search_Posts_Create_Between_Yesterday_And_Tomorrow()
        {
            var mockServiceLocator = new Mock<IServiceLocator>();

            mockServiceLocator
                .Setup(c => c.GetInstance<IAdvancedPostSearch>())
                .Returns(new AdvancedPostSearch(SessionFactory))
                .Verifiable();

            IServiceLocator serviceLocator = mockServiceLocator.Object;

            var repository = new NHibernateRepository<Post>(SessionFactory, new QueryFactory(serviceLocator));

            LoadPosts(repository);

            var query = repository.CreateQuery<IAdvancedPostSearch>();
            query.CreatedBetween = new Period(DateTime.Today.AddDays(-1), DateTime.Today.AddDays(1));

            IEnumerable<Post> posts = query.Execute();

            Assert.AreEqual(10, posts.Count());

            mockServiceLocator.VerifyAll();
        }
    }
}