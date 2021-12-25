using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using Moq;
using Repository;
using Service.UserService;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Test
{
    [TestClass]
    public class UserServiceTest
    {
        private IUserService userService;
        private Mock<IRepositoryService> repositoryServiceMock;

        [TestInitialize]
        public void InitTest()
        {
            repositoryServiceMock = new Mock<IRepositoryService>();
            userService = new UserService(repositoryServiceMock.Object);
        }


        [TestMethod]
        public void TestAddUser()
        {
            var user = new User { Username = "user1", Password = "password" };
            repositoryServiceMock.Setup(x => x.Add<User>(user)).Returns(user);

            Assert.AreEqual(userService.Add("user1", "password").Username, "user1");
        }

        [TestMethod]
        public void TestPasswordIsEncrypted()
        {
            var sha1 = new SHA1CryptoServiceProvider();
            var sha1data = sha1.ComputeHash(Encoding.ASCII.GetBytes("password"));
            var hashedPassword = Convert.ToBase64String(sha1data);
            
            var user = new User { Username = "user1", Password = hashedPassword };
            repositoryServiceMock.Setup(x => x.Add<User>(user)).Returns(user);

            Assert.AreEqual(userService.Add("user1", "password").Password, hashedPassword);
        }
    }
}
