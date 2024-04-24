using Lesson11.Models;
using Lesson11.Services;
using Lesson11.ViewModels;
using Newtonsoft.Json;

namespace Lesson11.Stores.User
{
    public class UserDataStore : IUserDataStore
    {
        private readonly ApiClient _apiClient;
        public UserDataStore(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public (bool Success, string Token) AuthenticateLogin(LoginViewModel loginViewModel)
        {
            var json = JsonConvert.SerializeObject(loginViewModel);
            var response = _apiClient.Post("auth/login", json);

            if (!response.IsSuccessStatusCode)
            {
                return (false, string.Empty);
            }

            var tokenJson = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var userData = JsonConvert.DeserializeObject<UserLogin>(tokenJson);

            return (true, userData.Token);
        }

        public (bool, string) RegisterLogin(RegisterViewModel registerViewModel)
        {
            var json = JsonConvert.SerializeObject(registerViewModel);
            var response = _apiClient.Post("auth/register", json);

            if (!response.IsSuccessStatusCode)
            {
                return (false, string.Empty);
            }

            var tokenJson = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var userData = JsonConvert.DeserializeObject<UserLogin>(tokenJson);

            return (true, userData.Token);
        }

    }

    public class AuthenticationResponse
    {
        public string Token { get; set; }
    }
}
