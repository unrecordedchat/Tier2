using System;
using System.ComponentModel.DataAnnotations;
namespace Entities.DTOs{
    
    public class MessageDTO
    {
        // Used for message creation or update
        [Required]
        public Guid SenderId { get; set; }

        public Guid? RecipientId { get; set; } // Nullable for group messages

        public Guid? GroupId { get; set; } // Nullable for direct messages

        [Required]
        public bool IsGroup { get; set; }

        [Required]
        public string ContentEncrypted { get; set; } // Content for creating or updating a message

        // Used in response for returning message details
        public Guid? Id { get; set; } // Nullable because it won't be set during creation

        public DateTime? Timestamp { get; set; } // Nullable to handle responses only

        public bool? IsDeleted { get; set; } // Nullable to handle responses only

        // For updating message content only
        public string NewContentEncrypted { get; set; } // For update requests (optional)

        // Convenience method to handle creation vs update
        public bool IsUpdate => !string.IsNullOrEmpty(NewContentEncrypted);
    }
}
