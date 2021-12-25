using JobsityChat.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using Moq;
using Service.PostService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace Test
{
    [TestClass]
    public class HomeControllerTest
    {
        private HomeController homeController;
        private Mock<IPostService> postServiceMock;

        [TestInitialize]
        public void InitTest()
        {
            postServiceMock = new Mock<IPostService>();
            homeController = new HomeController(postServiceMock.Object);

            var fakeHttpContext = new Mock<HttpContextBase>();
            var fakeIdentity = new GenericIdentity("User");
            var principal = new GenericPrincipal(fakeIdentity, null);

            fakeHttpContext.Setup(t => t.User).Returns(principal);
            var controllerContext = new Mock<ControllerContext>();
            controllerContext.Setup(t => t.HttpContext).Returns(fakeHttpContext.Object);
            homeController.ControllerContext = controllerContext.Object;
        }


        [TestMethod]
        public void TestListPostsEmpty()
        {
            postServiceMock.Setup(x => x.List(It.IsAny<int>(), It.IsAny<int>())).Returns(new List<Post>());

            var result = homeController.Chat() as ViewResult;
            Assert.AreEqual(((IList<Post>)result.Model).Count, 0);
        }

        [TestMethod]
        public void TestListPostsWithPosts()
        {
            var post1 = new Post { User = "user1", Message = "first", Date = DateTime.Now.AddSeconds(-10) };
            var post2 = new Post { User = "user2", Message = "second", Date = DateTime.Now.AddSeconds(-20) };

            postServiceMock.Setup(x => x.List(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(new List<Post> { post1, post2 });
                
            var result = homeController.Chat() as ViewResult;
            Assert.AreEqual(((IList<Post>)result.Model).Count, 2);
        }

        [TestMethod]
        public void TestListPostsOrder()
        {
            var post1 = new Post { User = "user1", Message = "first", Date = DateTime.Now.AddSeconds(-10) };
            var post2 = new Post { User = "user2", Message = "second", Date = DateTime.Now.AddSeconds(-20) };

            postServiceMock.Setup(x => x.List(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(new List<Post> { post1, post2 });

            var result = homeController.Chat() as ViewResult;
            Assert.AreEqual(((IList<Post>)result.Model).First().User, "user1");
        }

        [TestMethod]
        public void TestUserName()
        {
            postServiceMock.Setup(x => x.List(It.IsAny<int>(), It.IsAny<int>())).Returns(new List<Post>());

            var result = homeController.Chat() as ViewResult;
            Assert.AreEqual("User", result.ViewData["Username"]);
        }
    }
}
