using Entities.DTOs;

namespace HTTPClient.HTTPMessageClient
{
    /// <summary>
    /// Defines the contract for interacting with message-related API endpoints.
    /// </summary>
    public interface IMessageClient
    {
        /// <summary>
        /// Asynchronously creates a new message.
        /// </summary>
        /// <param name="message">The <see cref="MessageDTO"/> containing the details of the message to be created.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="message"/> is null.</exception>
        Task CreateMessageAsync(MessageDTO message);

        /// <summary>
        /// Retrieves a message by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the message to retrieve.</param>
        /// <returns>
        /// A task representing the asynchronous operation. 
        /// If successful, contains the <see cref="MessageDTO"/> for the specified ID; otherwise, null if the message does not exist.
        /// </returns>
        Task<MessageDTO?> GetMessageByIdAsync(Guid id);

        /// <summary>
        /// Retrieves all messages exchanged between two users.
        /// </summary>
        /// <param name="senderId">The unique identifier of the sender.</param>
        /// <param name="recipientId">The unique identifier of the recipient.</param>
        /// <returns>
        /// A task representing the asynchronous operation. Contains an <see cref="IEnumerable{T}"/> of <see cref="MessageDTO"/>.
        /// </returns>
        Task<IEnumerable<MessageDTO>> GetMessagesBetweenUsersAsync(Guid senderId, Guid recipientId);

        /// <summary>
        /// Retrieves all messages sent to a specific group.
        /// </summary>
        /// <param name="groupId">The unique identifier of the group.</param>
        /// <returns>
        /// A task representing the asynchronous operation. Contains an <see cref="IEnumerable{T}"/> of <see cref="MessageDTO"/>.
        /// </returns>
        Task<IEnumerable<MessageDTO>> GetMessagesForGroupAsync(Guid groupId);

        /// <summary>
        /// Updates the content of a specific message.
        /// </summary>
        /// <param name="id">The unique identifier of the message.</param>
        /// <param name="newContentEncrypted">The new encrypted content to set for the message.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="newContentEncrypted"/> is null or empty.
        /// </exception>
        Task UpdateMessageContentAsync(Guid id, string newContentEncrypted);

        /// <summary>
        /// Permanently deletes a message by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the message to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeleteMessageAsync(Guid id);

        /// <summary>
        /// Marks a message as deleted without permanently removing it.
        /// </summary>
        /// <param name="id">The unique identifier of the message to softly delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task SoftDeleteMessageAsync(Guid id);
    }
}