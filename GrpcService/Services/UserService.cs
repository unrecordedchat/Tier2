using Domain.Services.User;
using Grpc.Core;

namespace GrpcService.Services.User
{
    public class UserServiceImpl : UserService.UserServiceBase
    {
        private readonly IUserRepo _user;

        public UserServiceImpl(IUserRepo user)
        {
            _user = user;
        }

        public override async Task<Empty> CreateUser(UserEntity request, ServerCallContext context)
        {
            await _user.CreateUserAsync(new Entities.UserEntity
            {
                UserId = Guid.Parse(request.Id),
                Username = request.Username,
                Email = request.Email,
                PublicKey = request.PublicKey,
                PrivateKey = request.PrivateKey,
                PassHash = request.PassHash
            });
            return new Empty();
        }

        public override async Task<UserEntity?> GetUserById(UserIdRequest request, ServerCallContext context)
        {
            var user = await _user.GetUserByIdAsync(Guid.Parse(request.Id));
            return user != null ? new UserEntity
            {
                Id = user.UserId.ToString(),
                Username = user.Username,
                Email = user.Email,
                PublicKey = user.PublicKey,
                PrivateKey = user.PrivateKey,
                PassHash = user.PassHash
            } : null;
        }

        public override async Task<UserEntity?> GetUserByUsername(UsernameRequest request, ServerCallContext context)
        {
            var user = await _user.GetUserByUsernameAsync(request.Username);
            return user != null ? new UserEntity
            {
                Id = user.UserId.ToString(),
                Username = user.Username,
                Email = user.Email,
                PublicKey = user.PublicKey,
                PrivateKey = user.PrivateKey,
                PassHash = user.PassHash
            } : null;
        }

        public override async Task<UserEntity?> GetUserByEmail(EmailRequest request, ServerCallContext context)
        {
            var user = await _user.GetUserByEmailAsync(request.Email);
            return user != null ? new UserEntity
            {
                Id = user.UserId.ToString(),
                Username = user.Username,
                Email = user.Email,
                PublicKey = user.PublicKey,
                PrivateKey = user.PrivateKey,
                PassHash = user.PassHash
            } : null;
        }

        public override async Task<Empty> UpdateUsername(UpdateUsernameRequest request, ServerCallContext context)
        {
            await _user.UpdateUsernameAsync(Guid.Parse(request.Id), request.Username);
            return new Empty();
        }

        public override async Task<Empty> UpdateEmail(UpdateEmailRequest request, ServerCallContext context)
        {
            await _user.UpdateEmailAsync(Guid.Parse(request.Id), request.Email);
            return new Empty();
        }

        public override async Task<Empty> UpdateKeys(UpdateKeysRequest request, ServerCallContext context)
        {
            await _user.UpdateKeysAsync(Guid.Parse(request.Id), request.PublicKey, request.PrivateKey);
            return new Empty();
        }

        public override async Task<Empty> DeleteUser(UserIdRequest request, ServerCallContext context)
        {
            await _user.DeleteUserAsync(Guid.Parse(request.Id));
            return new Empty();
        }

        public override async Task<Empty> UpdatePassword(UpdatePasswordRequest request, ServerCallContext context)
        {
            await _user.UpdatePasswordAsync(Guid.Parse(request.Id), request.Password);
            return new Empty();
        }

        public override async Task<VerifyPasswordResponse> VerifyPassword(VerifyPasswordRequest request, ServerCallContext context)
        {
            var isValid = await _user.VerifyPasswordAsync(Guid.Parse(request.Id), request.Password);
            return new VerifyPasswordResponse { IsValid = isValid };
        }
    }
}