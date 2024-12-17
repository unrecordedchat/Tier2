using Entities;

namespace HTTPClient.IClient
{
    public interface IUserClient
    {
        Task CreateUserAsync(UserEntity user);
        Task LoginUserAsync(string username, string password);
        Task LoginUserByEmailAsync(string email, string password);
        Task<UserEntity?> GetUserByUsernameAsync(string username);
        Task<UserEntity?> GetUserByEmailAsync(string email);
        Task<UserEntity?> UpdateUsernameAsync(Guid userId, string username);
        Task<UserEntity?> UpdateEmailAsync(Guid userId, string email);
        Task<UserEntity?> DeleteUserAsync(Guid userId);
        Task<UserEntity?> UpdatePasswordAsync(Guid userId, string password);
    }
}