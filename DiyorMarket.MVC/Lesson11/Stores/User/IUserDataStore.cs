using Lesson11.Models;
using Lesson11.ViewModels;

namespace Lesson11.Stores.User
{
    public interface IUserDataStore
    {
        public (bool Success, string Token) AuthenticateLogin(LoginViewModel user);
        public (bool, string) RegisterLogin(RegisterViewModel user);
    }
}
