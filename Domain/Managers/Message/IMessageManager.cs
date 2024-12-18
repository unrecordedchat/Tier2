using Entities.DTOs;

namespace Domain.Managers.Message
{
    /// <summary>
    /// Defines the contract for managing messages in the domain.
    /// </summary>
    public interface IMessageManager
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
        /// <param name="id">The unique identifier for the message.</param>
        /// <returns>
        /// A task representing the asynchronous operation. If successful, contains the <see cref="MessageDTO"/> for the specified ID;
        /// otherwise, null if the message does not exist.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="id"/> is an empty GUID.</exception>
        Task<MessageDTO?> GetMessageByIdAsync(Guid id);

        /// <summary>
        /// Retrieves all messages exchanged between two users.
        /// </summary>
        /// <param name="senderId">The unique identifier of the message sender.</param>
        /// <param name="recipientId">The unique identifier of the message recipient.</param>
        /// <returns>
        /// A task representing the asynchronous operation. If successful, contains a collection of <see cref="MessageDTO"/> objects
        /// representing messages between the specified users.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown when either <paramref name="senderId"/> or <paramref name="recipientId"/> is an empty GUID.
        /// </exception>
        Task<IEnumerable<MessageDTO>> GetMessagesBetweenUsersAsync(Guid senderId, Guid recipientId);

        /// <summary>
        /// Retrieves all messages for a specific group.
        /// </summary>
        /// <param name="groupId">The unique identifier of the group.</param>
        /// <returns>
        /// A task representing the asynchronous operation. If successful, contains a collection of <see cref="MessageDTO"/> objects
        /// representing messages for the specified group.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="groupId"/> is an empty GUID.</exception>
        Task<IEnumerable<MessageDTO>> GetMessagesForGroupAsync(Guid groupId);

        /// <summary>
        /// Updates the content of an existing message.
        /// </summary>
        /// <param name="id">The unique identifier of the message to update.</param>
        /// <param name="newContentEncrypted">The new encrypted content to set for the message.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="id"/> is an empty GUID or <paramref name="newContentEncrypted"/> is null or whitespace.
        /// </exception>
        Task UpdateMessageContentAsync(Guid id, string newContentEncrypted);

        /// <summary>
        /// Permanently deletes a message by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the message to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="id"/> is an empty GUID.</exception>
        Task DeleteMessageAsync(Guid id);

        /// <summary>
        /// Marks a message as deleted without permanently deleting it.
        /// </summary>
        /// <param name="id">The unique identifier of the message to softly delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="id"/> is an empty GUID.</exception>
        Task SoftDeleteMessageAsync(Guid id);
    }
}