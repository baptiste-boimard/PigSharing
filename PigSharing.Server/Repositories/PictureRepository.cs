using Microsoft.EntityFrameworkCore;
using PigSharing.Server.Database;
using PigSharing.Server.Models;

namespace PigSharing.Server.Repositories;

public class PictureRepository
{
    private readonly PostgresDbContext _postgresDbContext;

    public PictureRepository(PostgresDbContext postgresDbContext)
    {
        _postgresDbContext = postgresDbContext;
    }

    public async Task<bool> Upload(string url, string publicId, Account account)
    {
        Picture newPicture = new Picture
        {
            Id = new Guid(),
            Url = url,
            PublicId = publicId,
            Private = false,
            Created = DateTime.UtcNow,
            AccountId = account.ConnectionToken,
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

    public async Task<Picture[]> GetAllPublics()
    {

        try
        {
            var urlList = await _postgresDbContext.Pictures
                .Where(p => p.Private == false)
                .ToArrayAsync();
        
            return urlList;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public async Task<Picture[]> GetAllImages(Guid idAccount)
    {
        try
        {
            var imagesArray = await _postgresDbContext.Pictures
                .Where(p=> p.AccountId == idAccount)
                .ToArrayAsync();

            return imagesArray;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<bool> UpdateStatusPrivate(Picture picture)
    {
        try
        {
            var result = _postgresDbContext.Pictures.Update(picture);
            await _postgresDbContext.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public async Task<bool> DeleteImage(Picture picture)
    {
        var handlePicture = await _postgresDbContext.Pictures.FindAsync(picture.Id);

        if (handlePicture != null)
        {
            _postgresDbContext.Pictures.Remove(handlePicture);
            await _postgresDbContext.SaveChangesAsync();

            return true;
        }

        return false;
    }
}