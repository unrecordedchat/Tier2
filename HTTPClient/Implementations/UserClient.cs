using HTTPClient.IServices;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Entities;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;

namespace HTTPClient.Implementations
{
    public class HttpClientException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public HttpClientException(string message, HttpStatusCode statusCode)
            : base(message)
        {
            StatusCode = statusCode;
        }
    }

    public class HttpUserClient : IUserClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<HttpUserClient> _logger;
        private readonly AsyncRetryPolicy<HttpResponseMessage> _retryPolicy;

        public HttpUserClient(HttpClient client, ILogger<HttpUserClient> logger)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            // Define retry policy with Polly
            _retryPolicy = Policy
                .Handle<HttpRequestException>()
                .OrResult<HttpResponseMessage>(response => !response.IsSuccessStatusCode)
                .WaitAndRetryAsync(
                    retryCount: 3,
                    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    onRetry: (response, timespan, retryCount, context) =>
                    {
                        _logger.LogWarning(
                            "Retry {RetryCount} for request. Reason: {Reason}",
                            retryCount,
                            response.Exception?.Message ?? response.Result?.StatusCode.ToString());
                    });
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
            _logger.LogError("HTTP Error: {StatusCode} - {Content}", response.StatusCode, errorContent);
            throw new HttpClientException($"Error: {errorContent}", response.StatusCode);
        }

        private void ValidateParameter(string param, string paramName)
        {
            if (string.IsNullOrWhiteSpace(param))
                throw new ArgumentException($"Parameter {paramName} cannot be null or empty.");
        }

        private void ValidateGuid(Guid param, string paramName)
        {
            if (param == Guid.Empty)
                throw new ArgumentException($"Parameter {paramName} cannot be an empty GUID.");
        }

        public async Task CreateUserAsync(object user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            var content = CreateJsonContent(user);
            var response = await _retryPolicy.ExecuteAsync(() => _client.PostAsync("/users", content));

            if (!response.IsSuccessStatusCode)
            {
                await HandleErrorResponseAsync(response);
            }
        }

        public async Task<UserEntity?> GetUserByIdAsync(Guid userId)
        {
            ValidateGuid(userId, nameof(userId));

            var response = await _retryPolicy.ExecuteAsync(() => _client.GetAsync($"/users/{userId}"));

            if (!response.IsSuccessStatusCode)
            {
                await HandleErrorResponseAsync(response);
            }

            return await DeserializeResponseAsync<UserEntity>(response);
        }

        public async Task<UserEntity?> GetUserByUsernameAsync(string username)
        {
            ValidateParameter(username, nameof(username));

            var response = await _retryPolicy.ExecuteAsync(() => _client.GetAsync($"/users/by-username?username={username}"));

            if (!response.IsSuccessStatusCode)
            {
                await HandleErrorResponseAsync(response);
            }

            return await DeserializeResponseAsync<UserEntity>(response);
        }

        public async Task<UserEntity?> GetUserByEmailAsync(string email)
        {
            ValidateParameter(email, nameof(email));

            var response = await _retryPolicy.ExecuteAsync(() => _client.GetAsync($"/users/by-email?email={email}"));

            if (!response.IsSuccessStatusCode)
            {
                await HandleErrorResponseAsync(response);
            }

            return await DeserializeResponseAsync<UserEntity>(response);
        }

        public async Task<UserEntity?> UpdateUsernameAsync(Guid userId, string username)
        {
            ValidateGuid(userId, nameof(userId));
            ValidateParameter(username, nameof(username));

            var content = CreateJsonContent(new { Username = username });
            var response = await _retryPolicy.ExecuteAsync(() => _client.PutAsync($"/users/{userId}/username", content));

            if (!response.IsSuccessStatusCode)
            {
                await HandleErrorResponseAsync(response);
            }

            return await DeserializeResponseAsync<UserEntity>(response);
        }

        public async Task<UserEntity?> UpdateEmailAsync(Guid userId, string email)
        {
            ValidateGuid(userId, nameof(userId));
            ValidateParameter(email, nameof(email));

            var content = CreateJsonContent(new { Email = email });
            var response = await _retryPolicy.ExecuteAsync(() => _client.PutAsync($"/users/{userId}/email", content));

            if (!response.IsSuccessStatusCode)
            {
                await HandleErrorResponseAsync(response);
            }

            return await DeserializeResponseAsync<UserEntity>(response);
        }

        public async Task<UserEntity?> UpdateKeysAsync(Guid userId, string publicKey, string privateKey)
        {
            ValidateGuid(userId, nameof(userId));
            ValidateParameter(publicKey, nameof(publicKey));
            ValidateParameter(privateKey, nameof(privateKey));

            var content = CreateJsonContent(new { PublicKey = publicKey, PrivateKey = privateKey });
            var response = await _retryPolicy.ExecuteAsync(() => _client.PutAsync($"/users/{userId}/keys", content));

            if (!response.IsSuccessStatusCode)
            {
                await HandleErrorResponseAsync(response);
            }

            return await DeserializeResponseAsync<UserEntity>(response);
        }

        public async Task<UserEntity?> DeleteUserAsync(Guid userId)
        {
            ValidateGuid(userId, nameof(userId));

            var response = await _retryPolicy.ExecuteAsync(() => _client.DeleteAsync($"/users/{userId}"));

            if (!response.IsSuccessStatusCode)
            {
                await HandleErrorResponseAsync(response);
            }

            return await DeserializeResponseAsync<UserEntity>(response);
        }

        public async Task<UserEntity?> UpdatePasswordAsync(Guid userId, string password)
        {
            ValidateGuid(userId, nameof(userId));
            ValidateParameter(password, nameof(password));

            var content = CreateJsonContent(new { Password = password });
            var response = await _retryPolicy.ExecuteAsync(() => _client.PutAsync($"/users/{userId}/password", content));

            if (!response.IsSuccessStatusCode)
            {
                await HandleErrorResponseAsync(response);
            }

            return await DeserializeResponseAsync<UserEntity>(response);
        }

        public async Task<bool> VerifyPasswordAsync(Guid userId, string password)
        {
            ValidateGuid(userId, nameof(userId));
            ValidateParameter(password, nameof(password));

            var content = CreateJsonContent(new { Password = password });
            var response = await _retryPolicy.ExecuteAsync(() => _client.PostAsync($"/users/{userId}/password/verify", content));

            if (!response.IsSuccessStatusCode)
            {
                await HandleErrorResponseAsync(response);
            }

            return await DeserializeResponseAsync<bool>(response);
        }
    }
}
