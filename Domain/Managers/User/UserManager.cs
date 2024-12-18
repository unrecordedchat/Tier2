using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Entities.DTOs;
using HTTPClient.HTTPUserClient;
using Microsoft.IdentityModel.Tokens;

namespace Domain.Managers.User
{
    /// <summary>
    /// Manages user-related operations, provides business logic, and ensures secure handling of user data.
    /// </summary>
    public class UserManager : IUserManager
    {
        private readonly IUserClient _userClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserManager"/> class.
        /// </summary>
        /// <param name="userClient">Client used to interact with user-related API endpoints.</param>
        public UserManager(IUserClient userClient)
        {
            _userClient = userClient;
        }

        /// <inheritdoc />
        public async Task CreateUserAsync(UserDTO user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user), "User object cannot be null.");

            if (string.IsNullOrWhiteSpace(user.Password))
                throw new ArgumentException("Password cannot be empty when creating a user.");

            if (string.IsNullOrWhiteSpace(user.Username))
                throw new ArgumentException("Username cannot be empty.");

            try
            {
                // Generate a unique GUID for the user ID if it doesn't exist
                user.Id = Guid.NewGuid().ToString(); // Keep as a GUID

                // Encrypt the password before proceeding
                user.Password = EncryptPassword(user.Password);

                // Check if the username already exists
                var existingUser = await _userClient.GetUserByUsernameAsync(user.Username);
                if (existingUser != null)
                {
                    throw new Exception($"A user with username '{user.Username}' already exists.");
                }

                // Proceed with user creation
                await _userClient.CreateUserAsync(user);
                Console.WriteLine("User created successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during user creation: {ex.Message}");
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<UserDTO?> GetUserByUsernameAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username cannot be null or empty.");

            try
            {
                return await _userClient.GetUserByUsernameAsync(username);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching user by username '{username}': {ex.Message}");
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<UserDTO?> GetUserByIdAsync(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("User ID cannot be empty.");
            
            try
            {
                return await _userClient.GetUserByIdAsync(userId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching user by ID '{userId}': {ex.Message}");
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<string> LoginUserAsync(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Username and password cannot be null or empty.");

            try
            {
                var user = await _userClient.GetUserByUsernameAsync(username);
                if (user == null)
                    throw new Exception("User not found.");

                var hashedInputPassword = EncryptPassword(password);
                if (user.Password != hashedInputPassword)
                {
                    throw new Exception("Invalid password.");
                }

                // Generate a JWT token
                var token = GenerateJwtToken(user);

                Console.WriteLine("Login successful.");
                return token;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during login for username '{username}': {ex.Message}");
                throw;
            }
        }

        private string GenerateJwtToken(UserDTO user)
        {
            // Define the security key (use a secret key that you store securely in production)
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourSecretKeyHere"));

            // Define the signing credentials (HS256 algorithm)
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Define the claims (additional information about the user)
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("userId", user.Id)
            };

            // Create the JWT token
            var token = new JwtSecurityToken(
                issuer: "YourIssuer",
                audience: "YourAudience",
                claims: claims,
                expires: DateTime.Now.AddHours(1), // Token expiration time (1 hour in this case)
                signingCredentials: credentials
            );

            // Generate and return the token string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <inheritdoc />
        public Task LogoutUserAsync(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("User ID cannot be empty.");

            try
            {
                Console.WriteLine("User logged out successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error logging out user ID '{userId}': {ex.Message}");
                throw;
            }
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public async Task UpdateUsernameAsync(Guid userId, string username)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("User ID cannot be empty.");

            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username cannot be empty.");

            try
            {
                await _userClient.UpdateUsernameAsync(userId, username);
                Console.WriteLine("Username updated successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating username for user ID '{userId}': {ex.Message}");
                throw;
            }
        }

        /// <inheritdoc />
        public async Task UpdateEmailAsync(Guid userId, string email)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("User ID cannot be empty.");

            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
                throw new ArgumentException("Email is invalid.");

            try
            {
                await _userClient.UpdateEmailAsync(userId, email);
                Console.WriteLine("Email updated successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating email for user ID '{userId}': {ex.Message}");
                throw;
            }
        }

        /// <inheritdoc />
        public async Task DeleteUserAsync(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("User ID cannot be empty.");

            try
            {
                await _userClient.DeleteUserAsync(userId);
                Console.WriteLine("User deleted successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting user ID '{userId}': {ex.Message}");
                throw;
            }
        }

        /// <inheritdoc />
        public async Task UpdatePasswordAsync(Guid userId, string password)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("User ID cannot be empty.");

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be empty.");

            try
            {
                // Encrypt the new password before updating
                var encryptedPassword = EncryptPassword(password);

                await _userClient.UpdatePasswordAsync(userId, encryptedPassword);
                Console.WriteLine("Password updated successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating password for user ID {userId}: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Encrypts a plain text password using SHA256.
        /// </summary>
        /// <param name="password">The plain text password to encrypt.</param>
        /// <returns>The hashed password in Base64 format.</returns>
        private string EncryptPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
