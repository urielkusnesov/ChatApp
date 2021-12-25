using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using Moq;
using Repository;
using Service.PostService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Test
{
    [TestClass]
    public class PostServiceTest
    {
        private IPostService postService;
        private Mock<IRepositoryService> repositoryServiceMock;

        [TestInitialize]
        public void InitTest()
        {
            repositoryServiceMock = new Mock<IRepositoryService>();
            postService = new PostService(repositoryServiceMock.Object);
        }


        [TestMethod]
        public void TestListPostsEmpty()
        {
            repositoryServiceMock.Setup(x => x.List<Post>(It.IsAny<Expression<Func<Post, bool>>>())).Returns(new List<Post>());

            Assert.IsTrue(postService.List().Count == 0);
        }

        [TestMethod]
        public void TestListPostsQuantity()
        {
            var post1 = new Post { User = "user1", Message = "first", Date = DateTime.Now.AddSeconds(-10) };
            var post2 = new Post { User = "user2", Message = "second", Date = DateTime.Now.AddSeconds(-20) };

            repositoryServiceMock.Setup(x => x.List<Post>(It.IsAny<Expression<Func<Post, bool>>>()))
                .Returns(new List<Post> { post1, post2 });

            Assert.AreEqual(postService.List().Count, 2);
        }

        [TestMethod]
        public void TestListPostsOrder()
        {
            var post1 = new Post { User = "user1", Message = "first", Date = DateTime.Now.AddSeconds(-10) };
            var post2 = new Post { User = "user2", Message = "second", Date = DateTime.Now.AddSeconds(-20) };

            repositoryServiceMock.Setup(x => x.List<Post>(It.IsAny<Expression<Func<Post, bool>>>()))
                .Returns(new List<Post> { post1, post2 });

            Assert.AreEqual(postService.List().First().User, "user1");
        }

        [TestMethod]
        public void TestListPostsOnly50()
        {
            var posts = new List<Post>();
            for(int i = 0; i < 60; i++)
            {
                posts.Add(new Post { User = "user", Message = "message", Date = DateTime.Now.AddSeconds(-i) });
            }

            repositoryServiceMock.Setup(x => x.List<Post>(It.IsAny<Expression<Func<Post, bool>>>())).Returns(posts);

            Assert.AreEqual(postService.List().Count, 50);
        }

        [TestMethod]
        public void TestAddPost()
        {
            var post = new Post { User = "user1", Message = "post message", Date = DateTime.Now };
            repositoryServiceMock.Setup(x => x.Add<Post>(post)).Returns(post);

            Assert.AreEqual(postService.Add(post).User, "user1");
        }
    }
}
