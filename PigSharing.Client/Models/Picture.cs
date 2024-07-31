
namespace PigSharing.Client.Models;

public class Picture
{
    public Guid Id { get; set; }
    public string Url { get; set; }
    public string PublicId { get; set; }
    public bool Private { get; set; }
    
    public DateTime Created { get; set; }
    public Guid AccountId { get; set; }
    public Account? Account { get; set; }
    
}