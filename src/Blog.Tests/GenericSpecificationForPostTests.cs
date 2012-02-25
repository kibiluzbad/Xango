using System.Collections.Generic;
using System.Linq;
using Blog.Domain;
using NUnit.Framework;
using Xango.Common.String;
using Xango.Data;
using Xango.Data.NHibernate;
using Xango.Data.NHibernate.Specifications;

namespace Blog.Tests
{
    [TestFixture]
    public class GenericSpecificationForPostTests : NHibernateBaseFixture
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
        public void Can_Select_One_Post_By_Title()
        {
            var repository = new NHibernateRepository<Post>(SessionFactory, null);

            LoadPosts(repository);

            Post post = repository.FindOne(new GenericSpecification<Post>(c => c.Title == "Title1"));

            Assert.AreEqual("Title1", post.Title);
            Assert.AreEqual("Body1", post.Body);
            Assert.AreEqual("Renata Fan", post.Author);
        }

        [Test]
        public void Can_Select_One_Post_By_Title_And_Author_Name()
        {
            var repository = new NHibernateRepository<Post>(SessionFactory, null);

            LoadPosts(repository);

            Post post = repository.FindOne(new GenericSpecification<Post>(c => c.Title == "Title1") &
                                           new GenericSpecification<Post>(c => c.Author.Contains("Renata Fan")));

            Assert.AreEqual("Title1", post.Title);
            Assert.AreEqual("Body1", post.Body);
            Assert.AreEqual("Renata Fan", post.Author);
        }

        [Test]
        public void Can_Select_Posts_By_Author_Name()
        {
            var repository = new NHibernateRepository<Post>(SessionFactory, null);

            LoadPosts(repository);

            IEnumerable<Post> posts =
                repository.FindAll(new GenericSpecification<Post>(c => c.Author.Contains("Renata Fan")));

            Assert.AreEqual(10, posts.Count());
        }
    }
}