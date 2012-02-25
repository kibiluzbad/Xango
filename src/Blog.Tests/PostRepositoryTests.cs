using System.Linq;
using Blog.Domain;
using NUnit.Framework;
using Xango.Common.Extensions;
using Xango.Common.String;
using Xango.Data.NHibernate;

namespace Blog.Tests
{
    [TestFixture]
    public class PostRepositoryTests : NHibernateBaseFixture
    {
        [Test]
        public void Can_Change_A_Saved_Post()
        {
            var newPost = new Post()
                              {
                                  Body = "Corpo do post!",
                                  Title = "Titulo do post",
                                  Slug = "Titulo do post".Slugify(),
                                  Author = "Renata Fan"
                              };

            var repository = new NHibernateRepository<Post>(SessionFactory, null);
            repository.Add(newPost);

            Session.Flush();

            Post saved = repository.FirstOrDefault(c => c.Id == newPost.Id);

            saved.Title += "Changed";
            saved.Body += "Changed";
            saved.Slug += saved.Title.Slugify();

            Session.Flush();

            Assert.AreEqual("Corpo do post!Changed", saved.Body);
            Assert.AreEqual("Titulo do postChanged", saved.Title);
        }

        [Test]
        public void Can_Comment_A_Saved_Post()
        {
            var newPost = new Post()
                              {
                                  Body = "Corpo do post!",
                                  Title = "Titulo do post",
                                  Slug = "Titulo do post".Slugify(),
                                  Author = "Renata Fan"
                              };

            var repository = new NHibernateRepository<Post>(SessionFactory, null);
            repository.Add(newPost);

            Session.Flush();

            Post saved = repository.FirstOrDefault(c => c.Id == newPost.Id);

            saved.Comment("Eeeeeeeeeeeu!", "Chapolin", "chapolin@colorado.com");

            Session.Flush();

            Assert.AreEqual(1, saved.Comments.Count());
            Assert.AreEqual("Chapolin", saved.Comments.FirstOrDefault().Author);
            Assert.AreEqual("Eeeeeeeeeeeu!", saved.Comments.FirstOrDefault().Text);
            Assert.AreEqual("chapolin@colorado.com", saved.Comments.FirstOrDefault().Mail);
        }

        [Test]
        public void Can_Create_A_New_Post()
        {
            var newPost = new Post()
                              {
                                  Body = "Corpo do post!",
                                  Title = "Titulo do post",
                                  Slug = "Titulo do post".Slugify(),
                                  Author = "Renata Fan"
                              };

            var repository = new NHibernateRepository<Post>(SessionFactory, null);
            repository.Add(newPost);

            Session.Flush();

            Assert.IsNotNull(newPost.Id);
            Assert.IsTrue(newPost.CreatedAt.IsToday());
            Assert.AreEqual("Corpo do post!", newPost.Body);
            Assert.AreEqual("Titulo do post", newPost.Title);
            Assert.AreEqual("Renata Fan", newPost.Author);
        }

        [Test]
        public void Can_Delete_A_Saved_Post()
        {
            var newPost = new Post()
                              {
                                  Body = "Corpo do post!",
                                  Title = "Titulo do post",
                                  Slug = "Titulo do post".Slugify(),
                                  Author = "Renata Fan"
                              };

            var repository = new NHibernateRepository<Post>(SessionFactory, null);
            repository.Add(newPost);

            Session.Flush();

            Post saved = repository.FirstOrDefault(c => c.Id == newPost.Id);

            repository.Remove(saved);

            Session.Flush();

            Assert.AreEqual(0, repository.Count);
        }
    }
}