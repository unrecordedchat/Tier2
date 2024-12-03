namespace Entities;

public class UserEntity
{
    public Guid? UserId { get; set; }
    public string Username { get; set; }
    public string PassHash { get; set; }
    public byte[] Salt { get; set; }
    public string Email { get; set; }
    public string PublicKey { get; set; }
    public string PrivateKey { get; set; }
}