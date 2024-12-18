using Domain.Managers.Friendship;
using Grpc.Core;

namespace GrpcService.Services
{
    /// <summary>
    /// gRPC service implementation for handling friendship-related operations.
    /// </summary>
    public class FriendshipServiceImpl : FriendshipService.FriendshipServiceBase
    {
        private readonly IFriendshipManager _friendshipManager;

        public FriendshipServiceImpl(IFriendshipManager friendshipManager)
        {
            _friendshipManager = friendshipManager ?? throw new ArgumentNullException(nameof(friendshipManager));
        }

        /// <inheritdoc />
        public override async Task<FriendshipResponseStatus> CreateFriendship(
            FriendshipStatusRequest request,
            ServerCallContext context)
        {
            try
            {
                if (!Guid.TryParse(request.UserId1, out var userId1) ||
                    !Guid.TryParse(request.UserId2, out var userId2))
                {
                    return new FriendshipResponseStatus
                    {
                        Success = false,
                        Message = "Invalid user ID(s)."
                    };
                }

                await _friendshipManager.CreateFriendshipAsync(userId1, userId2, request.Status);

                return new FriendshipResponseStatus
                {
                    Success = true,
                    Message = "Friendship created successfully."
                };
            }
            catch (Exception ex)
            {
                return new FriendshipResponseStatus
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        /// <inheritdoc />
        public override async Task<FriendshipDTO> GetFriendship(
            FriendshipRequest request,
            ServerCallContext context)
        {
            if (!Guid.TryParse(request.UserId1, out var userId1) ||
                !Guid.TryParse(request.UserId2, out var userId2))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid user ID(s)."));
            }

            var friendship = await _friendshipManager.GetFriendshipAsync(userId1, userId2);

            if (friendship == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Friendship not found."));
            }

            return new FriendshipDTO
            {
                Id = friendship.Id.ToString(),
                UserId1 = friendship.UserId1.ToString(),
                UserId2 = friendship.UserId2.ToString(),
                Status = friendship.Status
            };
        }

        /// <inheritdoc />
        public override async Task<FriendshipListResponse> GetAllFriendships(
            FriendshipUserIdRequest request,
            ServerCallContext context)
        {
            if (!Guid.TryParse(request.UserId, out var userId))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid user ID."));
            }

            var friendships = await _friendshipManager.GetAllFriendshipsAsync(userId);

            return new FriendshipListResponse
            {
                Friendships =
                {
                    friendships.Select(f => new FriendshipDTO
                    {
                        Id = f.Id.ToString(),
                        UserId1 = f.UserId1.ToString(),
                        UserId2 = f.UserId2.ToString(),
                        Status = f.Status
                    })
                }
            };
        }

        /// <inheritdoc />
        public override async Task<FriendshipResponseStatus> UpdateFriendshipStatus(
            FriendshipStatusRequest request,
            ServerCallContext context)
        {
            try
            {
                if (!Guid.TryParse(request.UserId1, out var userId1) ||
                    !Guid.TryParse(request.UserId2, out var userId2))
                {
                    return new FriendshipResponseStatus
                    {
                        Success = false,
                        Message = "Invalid user ID(s)."
                    };
                }

                await _friendshipManager.UpdateFriendshipStatusAsync(userId1, userId2, request.Status);

                return new FriendshipResponseStatus
                {
                    Success = true,
                    Message = "Friendship status updated successfully."
                };
            }
            catch (Exception ex)
            {
                return new FriendshipResponseStatus
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        /// <inheritdoc />
        public override async Task<FriendshipResponseStatus> DeleteFriendship(
            FriendshipRequest request,
            ServerCallContext context)
        {
            try
            {
                if (!Guid.TryParse(request.UserId1, out var userId1) ||
                    !Guid.TryParse(request.UserId2, out var userId2))
                {
                    return new FriendshipResponseStatus
                    {
                        Success = false,
                        Message = "Invalid user ID(s)."
                    };
                }

                await _friendshipManager.DeleteFriendshipAsync(userId1, userId2);

                return new FriendshipResponseStatus
                {
                    Success = true,
                    Message = "Friendship deleted successfully."
                };
            }
            catch (Exception ex)
            {
                return new FriendshipResponseStatus
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
    }
}