using Entities;
namespace Domain.Services.User
{
    public interface IUserRepo
    {
        Task CreateUserAsync(UserEntity user);
        Task<UserEntity?> GetUserByIdAsync(Guid userId);
        Task<UserEntity?> GetUserByUsernameAsync(string username);
        Task<UserEntity?> GetUserByEmailAsync(string email);
        Task UpdateUsernameAsync(Guid userId, string username);
        Task UpdateEmailAsync(Guid userId, string email);
        Task UpdateKeysAsync(Guid userId, string publicKey, string privateKey);
        Task DeleteUserAsync(Guid userId);
        Task UpdatePasswordAsync(Guid userId, string password);
        Task<bool> VerifyPasswordAsync(Guid userId, string password);
    }
}