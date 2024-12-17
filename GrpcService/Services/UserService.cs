using Domain.Managers.User;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
namespace GrpcService.Services
{
    public class UserServiceImpl : UserService.UserServiceBase
    {
        private readonly IUserManager _user;

        public UserServiceImpl(IUserManager user)
        {
            _user = user;
        }

        public override async Task<Empty> CreateUser(UserEntity request, ServerCallContext context)
        {
            await _user.CreateUserAsync(new Entities.UserEntity
            {
                Username = request.Username,
                Email = request.Email,
                Password = request.PassHash
            });
            return new Empty();
        }

        public override async Task<UserEntity?> GetUserByUsername(UsernameRequest request, ServerCallContext context)
        {
            var user = await _user.GetUserByUsernameAsync(request.Username);
            return user != null ? new UserEntity
            {
                Username = user.Username,
                Email = user.Email,
                PassHash = user.Password
            } : null;
        }

        public override async Task<UserEntity?> GetUserByEmail(EmailRequest request, ServerCallContext context)
        {
            var user = await _user.GetUserByEmailAsync(request.Email);
            return user != null ? new UserEntity
            {
                Username = user.Username,
                Email = user.Email,
                PassHash = user.Password
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
        
    }
}