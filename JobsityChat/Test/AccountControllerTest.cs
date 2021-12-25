using JobsityChat.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using Moq;
using Service.UserService;
using System.Web.Mvc;
using System.Web.Security;

namespace Test
{
    [TestClass]
    public class AccountControllerTest
    {
        private AccountController accountController;
        private Mock<IUserService> userServiceMock;

        [TestInitialize]
        public void InitTest()
        {
            userServiceMock = new Mock<IUserService>();
            accountController = new AccountController(userServiceMock.Object);
        }

        [TestMethod]
        public void TestLoginError()
        {
            var user = new User { Username = "user", Password = "password" };
            var result = accountController.Login(user, "") as ViewResult;
            Assert.AreEqual("user", ((User)result.Model).Username);
            Assert.AreEqual("Invalid user or password", result.ViewData.ModelState[""].Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void TestRegisterSuccess()
        {
            var user = new User { Username = "user", Password = "password" };
            var result = accountController.Register(user, "password") as RedirectToRouteResult;
            Assert.AreEqual("Index", result.RouteValues["Action"]);
            Assert.AreEqual("Home", result.RouteValues["Controller"]);
        }

        [TestMethod]
        public void TestRegisterPasswordDontMatch()
        {
            var user = new User { Username = "user", Password = "password" };
            var result = accountController.Register(user, "other") as ViewResult;
            Assert.AreEqual("user", ((User)result.Model).Username);
            Assert.AreEqual("Passwords do not match", result.ViewData.ModelState[""].Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void TestRegisterInvalidModel()
        {
            var user = new User { Username = "user", Password = "password" };
            accountController.ModelState.AddModelError("", "error");
            var result = accountController.Register(user, "password") as ViewResult;
            Assert.AreEqual("user", ((User)result.Model).Username);
            Assert.AreEqual("Invalid registration", result.ViewData.ModelState[""].Errors[1].ErrorMessage);
        }

        [TestMethod]
        public void TestRegisterServiceFailed()
        {
            var user = new User { Username = "user", Password = "password" };
            userServiceMock.Setup(x => x.Add(It.IsAny<string>(), It.IsAny<string>())).Throws(new MembershipCreateUserException());
            var result = accountController.Register(user, "password") as ViewResult;
            Assert.AreEqual("user", ((User)result.Model).Username);
            Assert.AreEqual("Cannot create user. Please try again", result.ViewData.ModelState[""].Errors[0].ErrorMessage);
        }
    }
}
