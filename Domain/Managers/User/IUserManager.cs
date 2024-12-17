using Entities;

namespace Domain.Managers.User
{
    public interface IUserManager
    {
        Task CreateUserAsync(UserEntity user);
        Task<UserEntity?> GetUserByUsernameAsync(string username);
        Task LoginUserAsync(string username, string password);
        Task LoginUserByEmailAsync(string email, string password);
        Task<UserEntity?> GetUserByEmailAsync(string email);
        Task UpdateUsernameAsync(Guid userId, string username);
        Task UpdateEmailAsync(Guid userId, string email);
        Task DeleteUserAsync(Guid userId);
        Task UpdatePasswordAsync(Guid userId, string password);
    }
}