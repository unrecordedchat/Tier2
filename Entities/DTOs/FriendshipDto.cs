namespace Entities.DTOs

{
    /// <summary>
    /// Data Transfer Object representing a friendship relationship between two users.
    /// Used for transferring data between layers in the application.
    /// </summary>
    public class FriendshipDto
    {
        /// <summary>
        /// The unique identifier for the friendship relationship.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The unique identifier for the first user in the friendship.
        /// </summary>
        public Guid UserId1 { get; set; }

        /// <summary>
        /// The unique identifier for the second user in the friendship.
        /// </summary>
        public Guid UserId2 { get; set; }

        /// <summary>
        /// The status of the friendship, such as 'FRD' (Friend), 'UNK' (Unknown), 'PND' (Pending).
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// Creates a string representation of the friendship DTO.
        /// </summary>
        public override string ToString()
        {
            return $"Friendship: User1={UserId1}, User2={UserId2}, Status={Status}";
        }
    }
}
