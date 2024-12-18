using System.Net.Http.Json;
using Entities.DTOs;

namespace HTTPClient.HTTPMessageClient
{
    /// <summary>
    /// Provides HTTP-based communication with the message-related API endpoints.
    /// </summary>
    public class MessageClient : IMessageClient
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageClient"/> class.
        /// </summary>
        /// <param name="httpClient">An instance of <see cref="HttpClient"/> for sending HTTP requests.</param>
        public MessageClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <inheritdoc />
        public async Task CreateMessageAsync(MessageDTO message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            var response = await _httpClient.PostAsJsonAsync("/api/messages", message);
            response.EnsureSuccessStatusCode();
        }

        /// <inheritdoc />
        public async Task<MessageDTO?> GetMessageByIdAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"/api/messages/{id}");
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<MessageDTO>();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<MessageDTO>> GetMessagesBetweenUsersAsync(Guid senderId, Guid recipientId)
        {
            var response = await _httpClient.GetAsync($"/api/messages/users/{senderId}/{recipientId}");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<IEnumerable<MessageDTO>>() ?? new List<MessageDTO>();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<MessageDTO>> GetMessagesForGroupAsync(Guid groupId)
        {
            var response = await _httpClient.GetAsync($"/api/messages/groups/{groupId}");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<IEnumerable<MessageDTO>>() ?? new List<MessageDTO>();
        }

        /// <inheritdoc />
        public async Task UpdateMessageContentAsync(Guid id, string newContentEncrypted)
        {
            if (string.IsNullOrEmpty(newContentEncrypted))
                throw new ArgumentException("Content for updating the message cannot be null or empty.", nameof(newContentEncrypted));

            var updatePayload = new MessageDTO
            {
                NewContentEncrypted = newContentEncrypted
            };

            var response = await _httpClient.PatchAsJsonAsync($"/api/messages/{id}", updatePayload);
            response.EnsureSuccessStatusCode();
        }

        /// <inheritdoc />
        public async Task DeleteMessageAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"/api/messages/{id}");
            response.EnsureSuccessStatusCode();
        }

        /// <inheritdoc />
        public async Task SoftDeleteMessageAsync(Guid id)
        {
            var response = await _httpClient.PostAsync($"/api/messages/{id}/softDelete", null);
            response.EnsureSuccessStatusCode();
        }
    }
}