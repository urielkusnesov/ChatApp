using Model;
using Repository;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Service.UserService
{
    public class UserService : IUserService
    {
        private readonly IRepositoryService repository;

        public UserService(IRepositoryService repository)
        {
            this.repository = repository;
        }

        public User Add(string username, string password)
        {
            if (!repository.Exists<User>(x => x.Username == username))
            {
                var sha1 = new SHA1CryptoServiceProvider();
                var sha1data = sha1.ComputeHash(Encoding.ASCII.GetBytes(password));
                var hashedPassword = Convert.ToBase64String(sha1data);
                var user = new User { Username = username, Password = hashedPassword };
                var result = repository.Add<User>(user);
                repository.SaveChanges();
                return user;
            }
            return null;
        }
    }
}
