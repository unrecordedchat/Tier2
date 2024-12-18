using Domain.Managers.User;
using Grpc.Core;

namespace GrpcService.Services
{
    public class UserServiceImpl(IUserManager userManager) : UserService.UserServiceBase
    {
        public override async Task<ResponseStatus> CreateUser(UserDTO request, ServerCallContext context)
        {
            try
            {
                await userManager.CreateUserAsync(new Entities.DTOs.UserDTO
                {
                    Username = request.Username,
                    Email = request.Email,
                    Password = request.Password,
                    PublicKey = request.PublicKey,
                    PrivateKeyEncrypted = request.PrivateKeyEncrypted
                });

                return new ResponseStatus { Success = true, Message = "User created successfully." };
            }
            catch (Exception ex)
            {
                return new ResponseStatus { Success = false, Message = ex.Message };
            }
        }

        public override async Task<UserDTO> GetUserByUsername(UsernameRequest request, ServerCallContext context)
        {
            var user = await userManager.GetUserByUsernameAsync(request.Username);
            if (user == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"User with username '{request.Username}' not found."));
            }

            return new UserDTO
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                PublicKey = user.PublicKey
            };
        }

        public override async Task<ResponseStatus> UpdateUsername(UpdateUsernameRequest request, ServerCallContext context)
        {
            try
            {
                await userManager.UpdateUsernameAsync(Guid.Parse(request.Id), request.Username);
                return new ResponseStatus { Success = true, Message = "Username updated successfully." };
            }
            catch (Exception ex)
            {
                return new ResponseStatus { Success = false, Message = ex.Message };
            }
        }

        public override async Task<ResponseStatus> UpdateEmail(UpdateEmailRequest request, ServerCallContext context)
        {
            try
            {
                await userManager.UpdateEmailAsync(Guid.Parse(request.Id), request.Email);
                return new ResponseStatus { Success = true, Message = "Email updated successfully." };
            }
            catch (Exception ex)
            {
                return new ResponseStatus { Success = false, Message = ex.Message };
            }
        }

        public override async Task<ResponseStatus> UpdatePassword(UpdatePasswordRequest request, ServerCallContext context)
        {
            try
            {
                await userManager.UpdatePasswordAsync(Guid.Parse(request.Id), request.Password);
                return new ResponseStatus { Success = true, Message = "Password updated successfully." };
            }
            catch (Exception ex)
            {
                return new ResponseStatus { Success = false, Message = ex.Message };
            }
        }

        public override async Task<ResponseStatus> DeleteUser(UserIdRequest request, ServerCallContext context)
        {
            try
            {
                await userManager.DeleteUserAsync(Guid.Parse(request.Id));
                return new ResponseStatus { Success = true, Message = "User deleted successfully." };
            }
            catch (Exception ex)
            {
                return new ResponseStatus { Success = false, Message = ex.Message };
            }
        }

        public override async Task<LoginResponse> LoginUser(LoginRequest request, ServerCallContext context)
        {
            try
            {
                var token = await userManager.LoginUserAsync(request.Username, request.Password);
                return new LoginResponse { Token = token };
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new RpcException(new Status(StatusCode.Unauthenticated, ex.Message));
            }
            catch (Exception ex)
            {
                throw new RpcException(new Status(StatusCode.Internal, ex.Message));
            }
        }

        public override async Task<ResponseStatus> LogoutUser(UserIdRequest request, ServerCallContext context)
        {
            try
            {
                await userManager.LogoutUserAsync(Guid.Parse(request.Id));
                return new ResponseStatus { Success = true, Message = "User logged out successfully." };
            }
            catch (Exception ex)
            {
                return new ResponseStatus { Success = false, Message = ex.Message };
            }
        }
    }
}
