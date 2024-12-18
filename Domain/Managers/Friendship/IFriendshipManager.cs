using Entities.DTOs;

namespace Domain.Managers.Friendship
{
    /// <summary>
    /// Interface for managing friendship-related operations.
    /// </summary>
    public interface IFriendshipManager
    {
        /// <summary>
        /// Creates a friendship between two users.
        /// </summary>
        /// <param name="userId1">The unique identifier of the first user.</param>
        /// <param name="userId2">The unique identifier of the second user.</param>
        /// <param name="status">The status of the friendship (e.g., 'FRD', 'PND').</param>
        /// <returns>A task that represents the asynchronous operation. Returns the created Friendship DTO.</returns>
        Task<FriendshipDto?> CreateFriendshipAsync(Guid userId1, Guid userId2, string status);

        /// <summary>
        /// Retrieves a friendship between two users.
        /// </summary>
        /// <param name="userId1">The unique identifier of the first user.</param>
        /// <param name="userId2">The unique identifier of the second user.</param>
        /// <returns>A task that represents the asynchronous operation. Returns the Friendship DTO if found.</returns>
        Task<FriendshipDto?> GetFriendshipAsync(Guid userId1, Guid userId2);

        /// <summary>
        /// Retrieves all friendships for a specific user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>A task that represents the asynchronous operation. Returns a list of Friendship DTOs.</returns>
        Task<List<FriendshipDto>> GetAllFriendshipsAsync(Guid userId);

        /// <summary>
        /// Updates the status of an existing friendship.
        /// </summary>
        /// <param name="userId1">The unique identifier of the first user.</param>
        /// <param name="userId2">The unique identifier of the second user.</param>
        /// <param name="newStatus">The new status of the friendship (e.g., 'FRD', 'BLK').</param>
        /// <returns>A task that represents the asynchronous operation. Returns the updated Friendship DTO.</returns>
        Task<FriendshipDto?> UpdateFriendshipStatusAsync(Guid userId1, Guid userId2, string newStatus);

        /// <summary>
        /// Deletes a friendship between two users.
        /// </summary>
        /// <param name="userId1">The unique identifier of the first user.</param>
        /// <param name="userId2">The unique identifier of the second user.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task DeleteFriendshipAsync(Guid userId1, Guid userId2);
    }
}