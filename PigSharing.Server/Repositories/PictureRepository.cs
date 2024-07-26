using Microsoft.AspNetCore.Components.Forms;
using PigSharing.Server.Database;
using PigSharing.Server.Service;
using PigSharing.Share.Models;

namespace PigSharing.Server.Repositories;

public class PictureRepository
{
    private readonly PostgresDbContext _postgresDbContext;

    public PictureRepository(PostgresDbContext postgresDbContext)
    {
        _postgresDbContext = postgresDbContext;
    }

    public async Task<bool> Upload(string url)
    {
        Picture newPicture = new Picture
        {
            Id = new Guid(),
            Url = url,
            Private = false,
        };

        try
        {
            _postgresDbContext.Add(newPicture);
            await _postgresDbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
       

        return true;

    }
    
}