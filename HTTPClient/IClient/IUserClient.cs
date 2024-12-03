using System.Threading.Tasks;
using Entities;

namespace HTTPClient.IServices
{
    public interface IUserClient
    {
        Task CreateUserAsync(object user);
        Task<UserEntity?> GetUserByIdAsync(Guid userId);
        Task<UserEntity?> GetUserByUsernameAsync(string username);
        Task<UserEntity?> GetUserByEmailAsync(string email);
        Task<UserEntity?> UpdateUsernameAsync(Guid userId, string username);
        Task<UserEntity?> UpdateEmailAsync(Guid userId, string email);
        Task<UserEntity?> UpdateKeysAsync(Guid userId, string publicKey, string privateKey);
        Task<UserEntity?> DeleteUserAsync(Guid userId);
        Task<UserEntity?> UpdatePasswordAsync(Guid userId, string password);
        Task<bool> VerifyPasswordAsync(Guid userId, string password);
    }
}