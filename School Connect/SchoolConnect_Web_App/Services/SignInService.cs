using SchoolConnect_Web_App.IServices;
using System.Linq.Expressions;
using System.Text;

namespace SchoolConnect_Web_App.Services
{
    public class SignInService : ISignInService
    {
        private readonly HttpClient _client;
        private const string BasePath = "/api/SignIn/";
        private Dictionary<string, object> _returnDictionary;

        public SignInService(HttpClient client)
        {
            _client = client;
            _returnDictionary = [];
        }

        public async Task<Dictionary<string, object>> SignInWithEmailAndPasswordAsync(string email, string password)
        {
            StringBuilder buildString = new();
            buildString.Append(BasePath);
            buildString.Append("SignIn/");

            throw new NotImplementedException();

        }

        public async Task SignOutAsync()
        {
            throw new NotImplementedException();
        }
    }
}
