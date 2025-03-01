
namespace PigSharing.Client.Models;

public class Account
{
    public Guid ConnectionToken { get; set; }
    public required string UserName { get; set; }
    public required string Password { get; set; }
    public byte[] Salt { get; set; } = Array.Empty<byte>();
    public List<Picture> Pictures { get; set; }
}