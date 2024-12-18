using Entities.DTOs;
using HTTPClient.HTTPMessageClient;
using Microsoft.Extensions.Logging;

namespace Domain.Managers.Message
{
    /// <summary>
    /// Provides business logic for managing messages, including creation, updates, and retrievals.
    /// </summary>
    public class MessageManager : IMessageManager
    {
        private readonly IMessageClient _messageClient;
        private readonly ILogger<MessageManager> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageManager"/> class.
        /// </summary>
        /// <param name="messageClient">The HTTP client to interact with messages.</param>
        /// <param name="logger">The logger instance for logging information, warnings, and errors.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="messageClient"/> or <paramref name="logger"/> is null.</exception>
        public MessageManager(IMessageClient messageClient, ILogger<MessageManager> logger)
        {
            _messageClient = messageClient ?? throw new ArgumentNullException(nameof(messageClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public async Task CreateMessageAsync(MessageDTO message)
        {
            try
            {
                if (message == null)
                    throw new ArgumentNullException(nameof(message));

                if (string.IsNullOrWhiteSpace(message.ContentEncrypted))
                    throw new ArgumentException("Message content cannot be empty.", nameof(message.ContentEncrypted));

                // Additional validation example: Validate message content length
                if (message.ContentEncrypted.Length > 1000)
                    throw new ArgumentException("Message content exceeds maximum allowed length of 1000 characters.", nameof(message.ContentEncrypted));
                    
                _logger.LogInformation("Attempting to create a new message for SenderId: {SenderId}, RecipientId: {RecipientId}", message.SenderId, message.RecipientId);

                await _messageClient.CreateMessageAsync(message);

                _logger.LogInformation("Message successfully created for SenderId: {SenderId}, RecipientId: {RecipientId}", message.SenderId, message.RecipientId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a message.");
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<MessageDTO?> GetMessageByIdAsync(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    throw new ArgumentException("Message ID cannot be empty.", nameof(id));

                _logger.LogInformation("Fetching message by ID: {MessageId}", id);

                var message = await _messageClient.GetMessageByIdAsync(id);

                if (message == null)
                {
                    _logger.LogWarning("No message found for ID: {MessageId}", id);
                }

                return message;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving message with ID: {MessageId}", id);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<MessageDTO>> GetMessagesBetweenUsersAsync(Guid senderId, Guid recipientId)
        {
            try
            {
                if (senderId == Guid.Empty || recipientId == Guid.Empty)
                    throw new ArgumentException("Sender ID or Recipient ID cannot be empty.");

                _logger.LogInformation("Fetching messages between SenderId: {SenderId} and RecipientId: {RecipientId}", senderId, recipientId);

                var messages = await _messageClient.GetMessagesBetweenUsersAsync(senderId, recipientId);

                return messages;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving messages between users.");
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<MessageDTO>> GetMessagesForGroupAsync(Guid groupId)
        {
            try
            {
                if (groupId == Guid.Empty)
                    throw new ArgumentException("Group ID cannot be empty.", nameof(groupId));

                _logger.LogInformation("Fetching messages for GroupId: {GroupId}", groupId);

                var messages = await _messageClient.GetMessagesForGroupAsync(groupId);

                return messages;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving messages for the group.");
                throw;
            }
        }

        /// <inheritdoc />
        public async Task UpdateMessageContentAsync(Guid id, string newContentEncrypted)
        {
            try
            {
                if (id == Guid.Empty)
                    throw new ArgumentException("Message ID cannot be empty.", nameof(id));

                if (string.IsNullOrWhiteSpace(newContentEncrypted))
                    throw new ArgumentException("Updated message content cannot be empty.", nameof(newContentEncrypted));

                _logger.LogInformation("Attempting to update message with ID: {MessageId}", id);

                await _messageClient.UpdateMessageContentAsync(id, newContentEncrypted);

                _logger.LogInformation("Message with ID: {MessageId} was successfully updated.", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating message content.");
                throw;
            }
        }

        /// <inheritdoc />
        public async Task DeleteMessageAsync(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    throw new ArgumentException("Message ID cannot be empty.", nameof(id));

                _logger.LogInformation("Attempting to delete message with ID: {MessageId}", id);

                await _messageClient.DeleteMessageAsync(id);

                _logger.LogInformation("Message with ID: {MessageId} was successfully deleted.", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the message.");
                throw;
            }
        }

        /// <inheritdoc />
        public async Task SoftDeleteMessageAsync(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    throw new ArgumentException("Message ID cannot be empty.", nameof(id));

                _logger.LogInformation("Attempting to soft-delete message with ID: {MessageId}", id);

                await _messageClient.SoftDeleteMessageAsync(id);

                _logger.LogInformation("Message with ID: {MessageId} was successfully soft-deleted.", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while soft-deleting the message.");
                throw;
            }
        }
    }
}