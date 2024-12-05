using Domain.Managers.User;
using Entities;
using HTTPClient.IClient;

namespace Domain.Services.User
{
    public class UserManager : IUserManager
    {
        private readonly IUserClient _userClient;

        public UserManager(IUserClient userClient)
        {
            _userClient = userClient;
        }

        public async Task CreateUserAsync(UserEntity user)
        {
            await _userClient.CreateUserAsync(user);
        }

        public async Task<UserEntity?> GetUserByIdAsync(Guid userId)
        {
            return await _userClient.GetUserByIdAsync(userId);
        }

        public async Task<UserEntity?> GetUserByUsernameAsync(string username)
        {
            return await _userClient.GetUserByUsernameAsync(username);
        }

        public async Task<UserEntity?> GetUserByEmailAsync(string email)
        {
            return await _userClient.GetUserByEmailAsync(email);
        }

        public async Task UpdateUsernameAsync(Guid userId, string username)
        {
            await _userClient.UpdateUsernameAsync(userId, username);
        }

        public async Task UpdateEmailAsync(Guid userId, string email)
        {
            await _userClient.UpdateEmailAsync(userId, email);
        }

        public async Task UpdateKeysAsync(Guid userId, string publicKey, string privateKey)
        {
            await _userClient.UpdateKeysAsync(userId, publicKey, privateKey);
        }

        public async Task DeleteUserAsync(Guid userId)
        {
            await _userClient.DeleteUserAsync(userId);
        }

        public async Task UpdatePasswordAsync(Guid userId, string password)
        {
            await _userClient.UpdatePasswordAsync(userId, password);
        }

        public async Task<bool> VerifyPasswordAsync(Guid userId, string password)
        {
            return await _userClient.VerifyPasswordAsync(userId, password);
        }
    }
}