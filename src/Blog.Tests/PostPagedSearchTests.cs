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
using Xango.Data.Query;

namespace Blog.Tests
{
    [TestFixture]
    public class PostPagedSearchTests : NHibernateBaseFixture
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
        public void Can_Get_Posts_First_Page()
        {
            var mockServiceLocator = new Mock<IServiceLocator>();

            mockServiceLocator
                .Setup(c => c.GetInstance<IPagedPostSearch>())
                .Returns(new PagedPostSearch(SessionFactory))
                .Verifiable();

            IServiceLocator serviceLocator = mockServiceLocator.Object;

            var repository = new NHibernateRepository<Post>(SessionFactory, new QueryFactory(serviceLocator));

            LoadPosts(repository);

            var query = repository.CreateQuery<IPagedPostSearch>();
            query.ItemsPerPage = 5;
            query.PageNumber = 1;

            PagedResult<Post> posts = query.Execute();

            Assert.AreEqual(10, posts.TotalItems);
            Assert.AreEqual(5, posts.PageOfResults.Count());

            mockServiceLocator.VerifyAll();
        }

        [Test]
        public void Can_Get_Posts_First_Page_By_Author_Name()
        {
            var mockServiceLocator = new Mock<IServiceLocator>();

            mockServiceLocator
                .Setup(c => c.GetInstance<IPagedPostSearch>())
                .Returns(new PagedPostSearch(SessionFactory))
                .Verifiable();

            IServiceLocator serviceLocator = mockServiceLocator.Object;

            var repository = new NHibernateRepository<Post>(SessionFactory, new QueryFactory(serviceLocator));

            LoadPosts(repository);

            var query = repository.CreateQuery<IPagedPostSearch>();
            query.ItemsPerPage = 5;
            query.PageNumber = 1;
            query.AuthorName = "Renata Fan";

            PagedResult<Post> posts = query.Execute();

            Assert.AreEqual(5, posts.TotalItems);
            Assert.AreEqual(5, posts.PageOfResults.Count());

            mockServiceLocator.VerifyAll();
        }
    }
}