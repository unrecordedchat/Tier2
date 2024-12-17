using System.Text;
using System.Text.Json;
using Entities;
using HTTPClient.IClient;
namespace HTTPClient.Implementations
{
    public class UserClient : IUserClient
    {
        private readonly HttpClient _client;


        public UserClient()
        {
            _client = Client.GetHttpClient();
        }

        private StringContent CreateJsonContent<T>(T obj) =>
            new StringContent(JsonSerializer.Serialize(obj), Encoding.UTF8, "application/json");

        private async Task<T?> DeserializeResponseAsync<T>(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        private async Task HandleErrorResponseAsync(HttpResponseMessage response)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Error: {response.StatusCode}, {response.ReasonPhrase}, {errorContent}");
        }

        public async Task CreateUserAsync(UserEntity user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            var content = CreateJsonContent(user);
            var response = await _client.PostAsync("/users", content);

            if (!response.IsSuccessStatusCode)
            {
                await HandleErrorResponseAsync(response);
            }
        }

        public async Task LoginUserAsync(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentNullException(nameof(username));
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentNullException(nameof(password));
            
            var content = CreateJsonContent(new { Username = username, Password = password });
            var response = await _client.PostAsync("/users/login", content);
            
            if (!response.IsSuccessStatusCode)
            {
                await HandleErrorResponseAsync(response);
            }
        }

        public async Task LoginUserByEmailAsync(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentNullException(nameof(email));
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentNullException(nameof(password));
            
            var content = CreateJsonContent(new { Email = email, Password = password });
            var response = await _client.PostAsync("/users/login", content);
            
            if (!response.IsSuccessStatusCode)
            {
                await HandleErrorResponseAsync(response);
            }
        }

        public async Task<UserEntity?> GetUserByUsernameAsync(string username)
        {

            var response = await  _client.GetAsync($"/users/by-username?username={username}");

            if (!response.IsSuccessStatusCode)
            {
                await HandleErrorResponseAsync(response);
            }

            return await DeserializeResponseAsync<UserEntity>(response);
        }

        public async Task<UserEntity?> GetUserByEmailAsync(string email)
        {
            var response = await _client.GetAsync($"/users/by-email?email={email}");

            if (!response.IsSuccessStatusCode)
            {
                await HandleErrorResponseAsync(response);
            }

            return await DeserializeResponseAsync<UserEntity>(response);
        }

        public async Task<UserEntity?> UpdateUsernameAsync(Guid userId, string username)
        {

            var content = CreateJsonContent(new { Username = username });
            var response = await _client.PutAsync($"/users/{userId}/username", content);

            if (!response.IsSuccessStatusCode)
            {
                await HandleErrorResponseAsync(response);
            }

            return await DeserializeResponseAsync<UserEntity>(response);
        }

        public async Task<UserEntity?> UpdateEmailAsync(Guid userId, string email)
        {
            var content = CreateJsonContent(new { Email = email });
            var response = await _client.PutAsync($"/users/{userId}/email", content);

            if (!response.IsSuccessStatusCode)
            {
                await HandleErrorResponseAsync(response);
            }

            return await DeserializeResponseAsync<UserEntity>(response);
        }
        

        public async Task<UserEntity?> DeleteUserAsync(Guid userId)
        {

            var response = await  _client.DeleteAsync($"/users/{userId}");

            if (!response.IsSuccessStatusCode)
            {
                await HandleErrorResponseAsync(response);
            }

            return await DeserializeResponseAsync<UserEntity>(response);
        }

        public async Task<UserEntity?> UpdatePasswordAsync(Guid userId, string password)
        {

            var content = CreateJsonContent(new { Password = password });
            var response = await _client.PutAsync($"/users/{userId}/password", content);

            if (!response.IsSuccessStatusCode)
            {
                await HandleErrorResponseAsync(response);
            }

            return await DeserializeResponseAsync<UserEntity>(response);
        }
    }
}
