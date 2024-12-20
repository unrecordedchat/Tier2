using Entities.DTOs;

namespace Domain.Managers.User
{
    /// <summary>
    /// Defines the contract for managing user-related operations.
    /// </summary>
    public interface IUserManager
    {
        /// <summary>
        /// Asynchronously creates a new user.
        /// </summary>
        /// <param name="user">The <see cref="UserDTO"/> containing the details of the user to be created.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="user"/> is null.</exception>
        Task CreateUserAsync(UserDTO user);

        /// <summary>
        /// Retrieves a user by their username.
        /// </summary>
        /// <param name="username">The username of the user to retrieve.</param>
        /// <returns>
        /// A task representing the asynchronous operation. If successful, contains the <see cref="UserDTO"/> for the specified username;
        /// otherwise, null if the user does not exist.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="username"/> is null or empty.</exception>
        Task<UserDTO?> GetUserByUsernameAsync(string username);

        /// <summary>
        /// Retrieves a user by their unique identifier.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to retrieve.</param>
        /// <returns>
        /// A task representing the asynchronous operation. If successful, contains the <see cref="UserDTO"/> for the specified ID;
        /// otherwise, null if the user does not exist.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="userId"/> is an empty GUID.</exception>
        Task<UserDTO?> GetUserByIdAsync(Guid userId);

        /// <summary>
        /// Logs in a user using their username and password.
        /// </summary>
        /// <param name="username">The username of the user attempting to log in.</param>
        /// <param name="password">The password of the user attempting to log in.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when either <paramref name="username"/> or <paramref name="password"/> is null or empty.
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when the login credentials are invalid.</exception>
        Task<string> LoginUserAsync(string username, string password);

        /// <summary>
        /// Logs out a user by their unique identifier.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to log out.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="userId"/> is an empty GUID.</exception>
        Task LogoutUserAsync(Guid userId);

        /// <summary>
        /// Updates the username of an existing user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user whose username is to be updated.</param>
        /// <param name="username">The new username to set.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="userId"/> is an empty GUID or <paramref name="username"/> is null or empty.
        /// </exception>
        Task UpdateUsernameAsync(Guid userId, string username);

        /// <summary>
        /// Updates the email address of an existing user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user whose email address is to be updated.</param>
        /// <param name="email">The new email address to set.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="userId"/> is an empty GUID or <paramref name="email"/> is null or invalid.
        /// </exception>
        Task UpdateEmailAsync(Guid userId, string email);

        /// <summary>
        /// Permanently deletes a user by their unique identifier.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="userId"/> is an empty GUID.</exception>
        Task DeleteUserAsync(Guid userId);

        /// <summary>
        /// Updates the password of an existing user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user whose password is to be updated.</param>
        /// <param name="password">The new password to set.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="userId"/> is an empty GUID or <paramref name="password"/> is null or empty.
        /// </exception>
        Task UpdatePasswordAsync(Guid userId, string password);
    }
}