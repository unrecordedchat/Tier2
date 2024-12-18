using System.Text;
using System.Text.Json;
using Entities.DTOs;

namespace HTTPClient.HTTPUserClient
{
    /// <summary>
    /// An implementation of <see cref="IUserClient"/> that communicates with the user service via HTTP.
    /// </summary>
    public class UserClient : IUserClient
    {
        private readonly HttpClient _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserClient"/> class using a pre-configured <see cref="HttpClient"/>.
        /// </summary>
        public UserClient()
        {
            _client = Client.GetHttpClient();
        }

        /// <summary>
        /// Serializes an object into a JSON <see cref="StringContent"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object to serialize.</typeparam>
        /// <param name="obj">The object to serialize.</param>
        /// <returns>A <see cref="StringContent"/> containing the serialized object in JSON format.</returns>
        private StringContent CreateJsonContent<T>(T obj) =>
            new StringContent(JsonSerializer.Serialize(obj), Encoding.UTF8, "application/json");

        /// <summary>
        /// Deserializes an HTTP response into a specified type.
        /// </summary>
        /// <typeparam name="T">The type to deserialize the response into.</typeparam>
        /// <param name="response">The HTTP response containing the serialized JSON.</param>
        /// <returns>
        /// A task representing the asynchronous operation. If successful, contains the deserialized object or null.
        /// </returns>
        private async Task<T?> DeserializeResponseAsync<T>(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        /// <summary>
        /// Throws an exception if the HTTP response indicates an error.
        /// </summary>
        /// <param name="response">The HTTP response to handle.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="HttpRequestException">
        /// Thrown when the response indicates a non-success status, containing detailed error information.
        /// </exception>
        private async Task HandleErrorResponseAsync(HttpResponseMessage response)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Error: {response.StatusCode}, {response.ReasonPhrase}, {errorContent}");
        }

        /// <inheritdoc />
        public async Task CreateUserAsync(UserDTO user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            var content = CreateJsonContent(user);
            var response = await _client.PostAsync("/api/users", content);

            if (!response.IsSuccessStatusCode)
            {
                await HandleErrorResponseAsync(response);
            }
        }

        /// <inheritdoc />
        public async Task<UserDTO?> GetUserByIdAsync(Guid userId)
        {
            var response = await _client.GetAsync($"/api/users/{userId}");

            if (!response.IsSuccessStatusCode)
            {
                await HandleErrorResponseAsync(response);
            }

            return await DeserializeResponseAsync<UserDTO>(response);
        }

        /// <inheritdoc />
        public async Task<UserDTO?> GetUserByUsernameAsync(string username)
        {
            var response = await _client.GetAsync($"/api/users/username/{username}");

            if (!response.IsSuccessStatusCode)
            {
                await HandleErrorResponseAsync(response);
            }

            return await DeserializeResponseAsync<UserDTO>(response);
        }

        /// <inheritdoc />
        public async Task UpdateUsernameAsync(Guid userId, string username)
        {
            var content = CreateJsonContent(new { Username = username });
            var response = await _client.PatchAsync($"/api/users/{userId}/username", content);

            if (!response.IsSuccessStatusCode)
            {
                await HandleErrorResponseAsync(response);
            }
        }

        /// <inheritdoc />
        public async Task UpdateEmailAsync(Guid userId, string email)
        {
            var content = CreateJsonContent(new { Email = email });
            var response = await _client.PatchAsync($"/api/users/{userId}/email", content);

            if (!response.IsSuccessStatusCode)
            {
                await HandleErrorResponseAsync(response);
            }
        }

        /// <inheritdoc />
        public async Task DeleteUserAsync(Guid userId)
        {
            var response = await _client.DeleteAsync($"/api/users/{userId}");

            if (!response.IsSuccessStatusCode)
            {
                await HandleErrorResponseAsync(response);
            }
        }

        /// <inheritdoc />
        public async Task UpdatePasswordAsync(Guid userId, string password)
        {
            var content = CreateJsonContent(new { Password = password });
            var response = await _client.PatchAsync($"/api/users/{userId}/password", content);

            if (!response.IsSuccessStatusCode)
            {
                await HandleErrorResponseAsync(response);
            }
        }

        /// <inheritdoc />
        public async Task<bool> VerifyPasswordAsync(string username, string password)
        {
            var content = CreateJsonContent(new { Username = username, Password = password });
            var response = await _client.PostAsync("/api/users/verifyPassword", content);

            if (!response.IsSuccessStatusCode)
            {
                await HandleErrorResponseAsync(response);
            }

            var result = await DeserializeResponseAsync<Dictionary<string, object>>(response);
            return result != null && result.ContainsKey("valid") && (bool)result["valid"];
        }
    }
}