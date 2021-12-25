using Model;

namespace Service.UserService
{
    public interface IUserService
    {
        User Add(string username, string password);
    }
}
