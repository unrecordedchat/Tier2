using Entities.DTOs;
using HTTPClient.HTTPFriendshipClient;
using Microsoft.Extensions.Logging;

namespace Domain.Managers.Friendship
{
    /// <summary>
    /// Manages friendship-related business logic.
    /// </summary>
    public class FriendshipManager : IFriendshipManager
    {
        private readonly IFriendshipClient _friendshipClient;
        private readonly ILogger<FriendshipManager> _logger;

        /// <summary>
        /// Constructs a new instance of the <see cref="FriendshipManager"/> class.
        /// </summary>
        /// <param name="friendshipClient">Client to interact with friendship-related APIs.</param>
        /// <param name="logger">Logger for tracking operations and errors.</param>
        public FriendshipManager(IFriendshipClient friendshipClient, ILogger<FriendshipManager> logger)
        {
            _friendshipClient = friendshipClient;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<FriendshipDto?> CreateFriendshipAsync(Guid userId1, Guid userId2, string status)
        {
            if (userId1 == Guid.Empty || userId2 == Guid.Empty)
                throw new ArgumentException("User IDs cannot be empty.");

            if (string.IsNullOrWhiteSpace(status))
                throw new ArgumentException("Friendship status cannot be null or empty.");

            try
            {
                var friendship = await _friendshipClient.CreateFriendshipAsync(userId1, userId2, status);
                _logger.LogInformation("Friendship created successfully between users {UserId1} and {UserId2}.", userId1, userId2);
                return friendship;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating friendship between users {UserId1} and {UserId2}.", userId1, userId2);
                throw; // Re-throwing the exception for the caller to handle
            }
        }

        /// <inheritdoc />
        public async Task<FriendshipDto?> GetFriendshipAsync(Guid userId1, Guid userId2)
        {
            if (userId1 == Guid.Empty || userId2 == Guid.Empty)
                throw new ArgumentException("User IDs cannot be empty.");

            try
            {
                var friendship = await _friendshipClient.GetFriendshipAsync(userId1, userId2);
                if (friendship != null)
                {
                    _logger.LogInformation("Friendship retrieved successfully between users {UserId1} and {UserId2}.", userId1, userId2);
                }
                return friendship;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving friendship between users {UserId1} and {UserId2}.", userId1, userId2);
                throw; // Re-throwing the exception for the caller to handle
            }
        }

        /// <inheritdoc />
        public async Task<List<FriendshipDto>> GetAllFriendshipsAsync(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("User ID cannot be empty.");

            try
            {
                var friendships = await _friendshipClient.GetAllFriendshipsForUserAsync(userId);
                _logger.LogInformation("Retrieved {Count} friendships for user {UserId}.", friendships.Count, userId);
                return friendships;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all friendships for user {UserId}.", userId);
                throw; // Re-throwing the exception for the caller to handle
            }
        }

        /// <inheritdoc />
        public async Task<FriendshipDto?> UpdateFriendshipStatusAsync(Guid userId1, Guid userId2, string newStatus)
        {
            if (userId1 == Guid.Empty || userId2 == Guid.Empty)
                throw new ArgumentException("User IDs cannot be empty.");

            if (string.IsNullOrWhiteSpace(newStatus))
                throw new ArgumentException("New friendship status cannot be null or empty.");

            try
            {
                var updatedFriendship = await _friendshipClient.UpdateFriendshipStatusAsync(userId1, userId2, newStatus);
                _logger.LogInformation("Friendship status updated successfully between users {UserId1} and {UserId2}.", userId1, userId2);
                return updatedFriendship;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating friendship status between users {UserId1} and {UserId2}.", userId1, userId2);
                throw; // Re-throwing the exception for the caller to handle
            }
        }

        /// <inheritdoc />
        public async Task DeleteFriendshipAsync(Guid userId1, Guid userId2)
        {
            if (userId1 == Guid.Empty || userId2 == Guid.Empty)
                throw new ArgumentException("User IDs cannot be empty.");

            try
            {
                await _friendshipClient.DeleteFriendshipAsync(userId1, userId2);
                _logger.LogInformation("Friendship deleted successfully between users {UserId1} and {UserId2}.", userId1, userId2);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting friendship between users {UserId1} and {UserId2}.", userId1, userId2);
                throw; // Re-throwing the exception for the caller to handle
            }
        }
    }
}