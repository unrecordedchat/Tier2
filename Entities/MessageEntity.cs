namespace Entities
{
    public class MessageEntity
    {
        public Guid? MessageId { get; set; }
        public Guid SenderId { get; set; }
        public Guid RecipientId { get; set; }
        public bool IsGroup { get; set; }
        public string ContentEncrypted { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public bool IsDeleted { get; set; }
    }
}