using Entities.DTOs;

namespace HTTPClient.HTTPUserClient
{
    /// <summary>
    /// Defines the contract for HTTP-based communication with the user service.
    /// </summary>
    public interface IUserClient
    {
        /// <summary>
        /// Asynchronously creates a new user.
        /// </summary>
        /// <param name="user">The <see cref="UserDTO"/> containing the details of the user to be created.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="user"/> is null.</exception>
        Task CreateUserAsync(UserDTO user);

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
        /// Updates the username of an existing user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user whose username is to be updated.</param>
        /// <param name="username">The new username to set for the user.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="userId"/> is an empty GUID or <paramref name="username"/> is null or empty.
        /// </exception>
        Task UpdateUsernameAsync(Guid userId, string username);

        /// <summary>
        /// Updates the email address of an existing user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user whose email address is to be updated.</param>
        /// <param name="email">The new email address to set for the user.</param>
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
        /// <param name="password">The new password to set for the user.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="userId"/> is an empty GUID or <paramref name="password"/> is null or empty.
        /// </exception>
        Task UpdatePasswordAsync(Guid userId, string password);

        /// <summary>
        /// Verifies if the given password matches the specified user's credentials.
        /// </summary>
        /// <param name="username">The username of the user attempting to verify their password.</param>
        /// <param name="password">The password to verify.</param>
        /// <returns>
        /// A task representing the asynchronous operation. Contains a boolean indicating whether the password is valid or not.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="username"/> or <paramref name="password"/> is null or empty.
        /// </exception>
        Task<bool> VerifyPasswordAsync(string username, string password);
    }
}