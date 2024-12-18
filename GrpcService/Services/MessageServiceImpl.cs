using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Domain.Managers.Message;

namespace GrpcService.Services
{
    public class MessageServiceImpl(IMessageManager messageManager) : MessageService.MessageServiceBase
    {
        private readonly IMessageManager _messageManager = messageManager ?? throw new ArgumentNullException(nameof(messageManager));

        // Create a new message
        public override async Task<Empty> CreateMessage(CreateMessageRequest request, ServerCallContext context)
        {
            if (request.Message == null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid message data."));
            }

            // Map GrpcService.MessageDTO to Entities.DTOs.MessageDTO
            var messageDto = new Entities.DTOs.MessageDTO
            {
                SenderId = Guid.Parse(request.Message.SenderId),
                RecipientId = string.IsNullOrWhiteSpace(request.Message.RecipientId) 
                              ? null 
                              : Guid.Parse(request.Message.RecipientId),
                GroupId = string.IsNullOrWhiteSpace(request.Message.GroupId) 
                          ? null 
                          : Guid.Parse(request.Message.GroupId),
                IsGroup = request.Message.IsGroup,
                ContentEncrypted = request.Message.ContentEncrypted,
                NewContentEncrypted = request.Message.NewContentEncrypted
            };

            await _messageManager.CreateMessageAsync(messageDto);

            return new Empty();
        }

        // Retrieve messages between two users
        public override async Task<MessageListResponse> GetMessagesBetweenUsers(GetMessagesBetweenUsersRequest request, ServerCallContext context)
        {
            if (string.IsNullOrWhiteSpace(request.SenderId) || string.IsNullOrWhiteSpace(request.RecipientId))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Sender ID and Recipient ID cannot be empty."));
            }

            var senderId = Guid.Parse(request.SenderId);
            var recipientId = Guid.Parse(request.RecipientId);

            var messages = await _messageManager.GetMessagesBetweenUsersAsync(senderId, recipientId);

            var response = new MessageListResponse
            {
                Messages =
                {
                    messages.Select(message => new GrpcService.MessageDTO
                    {
                        Id = message.Id?.ToString(),
                        SenderId = message.SenderId.ToString(),
                        RecipientId = message.RecipientId?.ToString(),
                        GroupId = message.GroupId?.ToString(),
                        IsGroup = message.IsGroup,
                        ContentEncrypted = message.ContentEncrypted,
                        Timestamp = Timestamp.FromDateTimeOffset(message.Timestamp ?? DateTimeOffset.UtcNow),
                        IsDeleted = message.IsDeleted ?? false
                    }).ToList()
                }
            };

            return response;
        }

        // Retrieve messages for a specific group
        public override async Task<MessageListResponse> GetMessagesForGroup(GetMessagesForGroupRequest request, ServerCallContext context)
        {
            if (string.IsNullOrWhiteSpace(request.GroupId))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Group ID cannot be empty."));
            }

            var groupId = Guid.Parse(request.GroupId);

            var messages = await _messageManager.GetMessagesForGroupAsync(groupId);

            var response = new MessageListResponse
            {
                Messages =
                {
                    messages.Select(message => new GrpcService.MessageDTO
                    {
                        Id = message.Id?.ToString(),
                        SenderId = message.SenderId.ToString(),
                        RecipientId = message.RecipientId?.ToString(),
                        GroupId = message.GroupId?.ToString(),
                        IsGroup = message.IsGroup,
                        ContentEncrypted = message.ContentEncrypted,
                        Timestamp = Timestamp.FromDateTimeOffset(message.Timestamp ?? DateTimeOffset.UtcNow),
                        IsDeleted = message.IsDeleted ?? false
                    }).ToList()
                }
            };

            return response;
        }

        // Update the content of a message
        public override async Task<Empty> UpdateMessageContent(UpdateMessageContentRequest request, ServerCallContext context)
        {
            if (string.IsNullOrWhiteSpace(request.MessageId) || string.IsNullOrWhiteSpace(request.NewContentEncrypted))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Message ID and new content are required."));
            }

            var messageId = Guid.Parse(request.MessageId);
            await _messageManager.UpdateMessageContentAsync(messageId, request.NewContentEncrypted);

            return new Empty();
        }

        // Permanently delete a message
        public override async Task<Empty> DeleteMessage(DeleteMessageRequest request, ServerCallContext context)
        {
            if (string.IsNullOrWhiteSpace(request.MessageId))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Message ID cannot be empty."));
            }

            var messageId = Guid.Parse(request.MessageId);
            await _messageManager.DeleteMessageAsync(messageId);

            return new Empty();
        }

        // Soft delete a message
        public override async Task<Empty> SoftDeleteMessage(SoftDeleteMessageRequest request, ServerCallContext context)
        {
            if (string.IsNullOrWhiteSpace(request.MessageId))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Message ID cannot be empty."));
            }

            var messageId = Guid.Parse(request.MessageId);
            await _messageManager.SoftDeleteMessageAsync(messageId);

            return new Empty();
        }
    }
}