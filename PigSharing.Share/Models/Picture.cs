namespace PigSharing.Share.Models;

public class Picture
{
    public Guid Id { get; set; }
    public string Url { get; set; }
    public bool Private { get; set; }
}