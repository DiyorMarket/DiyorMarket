using Lesson11.Models;

namespace Lesson11.Stores.User
{
    public interface IUserDataStore
    {
        public (bool Success, string Token) AuthenticateLogin(UserLogin user);
        public (bool, string) RegisterLogin(UserLogin user);
    }
}
