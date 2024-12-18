using System.Net.Http.Json;
using System.Text.Json;
using Entities.DTOs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace HTTPClient.HTTPFriendshipClient
{
    // Custom exception for the FriendshipClient
    public class FriendshipClientException : Exception
    {
        public FriendshipClientException(string message) : base(message) { }
        public FriendshipClientException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class FriendshipClient : IFriendshipClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseRoute;
        private readonly ILogger<FriendshipClient> _logger;

        public FriendshipClient(HttpClient httpClient, IConfiguration configuration, ILogger<FriendshipClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            // Configurable routes, fallback to default value if not provided
            _baseRoute = configuration["FriendshipApi:BaseRoute"] ?? "api/friendships";
        }

        // 1. Create a new friendship (POST /api/friendships)
        public async Task<FriendshipDto?> CreateFriendshipAsync(Guid userId1, Guid userId2, string status, CancellationToken cancellationToken = default)
        {
            try
            {
                var requestBody = new { userId1 = userId1.ToString(), userId2 = userId2.ToString(), status };
                var response = await _httpClient.PostAsJsonAsync($"{_baseRoute}", requestBody, cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<FriendshipDto>(cancellationToken: cancellationToken);
                }

                var error = await response.Content.ReadAsStringAsync(cancellationToken);
                throw new FriendshipClientException($"Error creating friendship: {error}");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Network error occurred while creating friendship.");
                throw new FriendshipClientException("A network error occurred.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while creating friendship.");
                throw new FriendshipClientException("An unexpected error occurred.", ex);
            }
        }

        // 2. Retrieve a specific friendship (GET /api/friendships/{userId1}/{userId2})
        public async Task<FriendshipDto?> GetFriendshipAsync(Guid userId1, Guid userId2, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseRoute}/{userId1}/{userId2}", cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<FriendshipDto>(cancellationToken: cancellationToken);
                }

                return null;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Network error occurred while retrieving a friendship.");
                throw new FriendshipClientException("A network error occurred.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while retrieving a friendship.");
                throw new FriendshipClientException("An unexpected error occurred.", ex);
            }
        }

        // 3. Retrieve all friendships for a user (GET /api/friendships/{userId})
        public async Task<List<FriendshipDto>> GetAllFriendshipsForUserAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseRoute}/{userId}", cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<List<FriendshipDto>>(cancellationToken: cancellationToken) 
                           ?? new List<FriendshipDto>();
                }

                var error = await response.Content.ReadAsStringAsync(cancellationToken);
                throw new FriendshipClientException($"Error retrieving friendships: {error}");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Network error occurred while retrieving friendships for user {UserId}.", userId);
                throw new FriendshipClientException("A network error occurred.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while retrieving friendships for user {UserId}.", userId);
                throw new FriendshipClientException("An unexpected error occurred.", ex);
            }
        }

        // 4. Update a friendship's status (PATCH /api/friendships/{userId1}/{userId2}/status)
        public async Task<FriendshipDto?> UpdateFriendshipStatusAsync(Guid userId1, Guid userId2, string newStatus, CancellationToken cancellationToken = default)
        {
            try
            {
                var requestBody = new { status = newStatus };
                var response = await _httpClient.PatchAsync($"{_baseRoute}/{userId1}/{userId2}/status", 
                                                            JsonContent.Create(requestBody), 
                                                            cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<FriendshipDto>(cancellationToken: cancellationToken);
                }

                var error = await response.Content.ReadAsStringAsync(cancellationToken);
                throw new FriendshipClientException($"Error updating friendship status: {error}");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Network error occurred while updating friendship status.");
                throw new FriendshipClientException("A network error occurred.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while updating friendship status.");
                throw new FriendshipClientException("An unexpected error occurred.", ex);
            }
        }

        // 5. Delete a friendship (DELETE /api/friendships/{userId1}/{userId2})
        public async Task DeleteFriendshipAsync(Guid userId1, Guid userId2, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_baseRoute}/{userId1}/{userId2}", cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync(cancellationToken);
                    throw new FriendshipClientException($"Error deleting friendship: {error}");
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Network error occurred while deleting a friendship.");
                throw new FriendshipClientException("A network error occurred.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while deleting a friendship.");
                throw new FriendshipClientException("An unexpected error occurred.", ex);
            }
        }
    }
}