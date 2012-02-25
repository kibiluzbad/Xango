using System.Collections.Generic;
using System.Linq;
using Blog.Domain;
using Blog.Repository.Queries;
using NUnit.Framework;
using Xango.Common.String;
using Xango.Data.NHibernate;

namespace Blog.Tests
{
    [TestFixture]
    public class PostCreatedByTests : NHibernateBaseFixture
    {
        private void LoadPosts(NHibernateRepository<Post> repository)
        {
            for (int i = 0; i < 10; i++)
            {
                var post = new Post()
                               {
                                   Body = "Body" + i,
                                   Title = "Title" + i,
                                   Slug = ("Title" + i).Slugify(),
                                   Author = "Renata Fan"
                               };

                repository.Add(post);
            }
            Session.Flush();
        }

        [Test]
        public void Can_Select_Posts_By_Author_Name()
        {
            var repository = new NHibernateRepository<Post>(SessionFactory, null);

            LoadPosts(repository);

            IEnumerable<Post> posts = repository.FindAll(new PostCreatedBy("Renata Fan"));

            Assert.AreEqual(10, posts.Count());
        }
    }
}