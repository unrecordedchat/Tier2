namespace Entities.DTOs
{
    /// <summary>
    /// Represents a user DTO that can handle creating, updating, and fetching user details.
    /// </summary>
    public class UserDTO
    {
        /// <summary>
        /// The unique identifier for the user. Exposed only when retrieving user details.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The username of the user.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The user's password, used for user creation only.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// The user's email address.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The user's public key for cryptographic purposes.
        /// </summary>
        public string PublicKey { get; set; }

        /// <summary>
        /// The user's encrypted private key, used for user creation only.
        /// </summary>
        public string PrivateKeyEncrypted { get; set; }

        // Constructor for creating a new user (with password and privateKeyEncrypted).
        public UserDTO(string username, string password, string email, string publicKey, string privateKeyEncrypted)
        {
            Username = username;
            Password = password;
            Email = email;
            PublicKey = publicKey;
            PrivateKeyEncrypted = privateKeyEncrypted;
        }

        // Constructor for updating user info (without password and privateKeyEncrypted).
        public UserDTO(string id, string username, string email, string publicKey)
        {
            Id = id;
            Username = username;
            Email = email;
            PublicKey = publicKey;
        }

        // Default constructor (useful for deserialization or when creating an empty object).
        public UserDTO() { }
    }
}