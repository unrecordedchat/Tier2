using Entities;
using HTTPClient.IClient;

namespace Domain.Managers.User
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
        
        public async Task<UserEntity?> GetUserByUsernameAsync(string username)
        {
            return await _userClient.GetUserByUsernameAsync(username);
        }

        public async Task LoginUserAsync(string username, string password)
        {
            await _userClient.LoginUserAsync(username, password);
        }

        public async Task LoginUserByEmailAsync(string email, string password)
        {
            await _userClient.LoginUserByEmailAsync(email, password);
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
        

        public async Task DeleteUserAsync(Guid userId)
        {
            await _userClient.DeleteUserAsync(userId);
        }

        public async Task UpdatePasswordAsync(Guid userId, string password)
        {
            await _userClient.UpdatePasswordAsync(userId, password);
        }
    }
}