using Entities.DTOs;

namespace HTTPClient.HTTPFriendshipClient
{
    /// <summary>
    /// Interface for managing friendship-related HTTP requests.
    /// </summary>
    public interface IFriendshipClient
    {
        /// <summary>
        /// Creates a new friendship between two users.
        /// </summary>
        /// <param name="userId1">The unique identifier of the first user.</param>
        /// <param name="userId2">The unique identifier of the second user.</param>
        /// <param name="status">The status of the friendship (e.g., 'FRD', 'UNK', 'PND').</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the created FriendshipDto.</returns>
        Task<FriendshipDto?> CreateFriendshipAsync(Guid userId1, Guid userId2, string status, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a friendship between two users.
        /// </summary>
        /// <param name="userId1">The unique identifier of the first user.</param>
        /// <param name="userId2">The unique identifier of the second user.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the FriendshipDto if found.</returns>
        Task<FriendshipDto?> GetFriendshipAsync(Guid userId1, Guid userId2, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves all friendships for a specific user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of FriendshipDto.</returns>
        Task<List<FriendshipDto>> GetAllFriendshipsForUserAsync(Guid userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates the status of a friendship.
        /// </summary>
        /// <param name="userId1">The unique identifier of the first user.</param>
        /// <param name="userId2">The unique identifier of the second user.</param>
        /// <param name="newStatus">The new status to assign to the friendship (e.g., 'FRD', 'UNK', 'PND').</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the updated FriendshipDto.</returns>
        Task<FriendshipDto?> UpdateFriendshipStatusAsync(Guid userId1, Guid userId2, string newStatus, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a friendship between two users.
        /// </summary>
        /// <param name="userId1">The unique identifier of the first user.</param>
        /// <param name="userId2">The unique identifier of the second user.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task DeleteFriendshipAsync(Guid userId1, Guid userId2, CancellationToken cancellationToken = default);
    }
}