using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using PigSharing.Server.Database;
using PigSharing.Server.Models;
using PigSharing.Server.Service;
using Payload = PigSharing.Server.Models.Payload;

namespace PigSharing.Server.Repositories;

public class AuthRepository
{
    private readonly PostgresDbContext _postgresDbContext;
    private readonly PictureService _pictureService;

    public AuthRepository(PostgresDbContext postgresDbContext, PictureService pictureService)
    {
        _postgresDbContext = postgresDbContext;
        _pictureService = pictureService;
    }

    // Enrengistrement de la BDD du new user si il n'est pas existant
    // ou non null
    public async Task<Account> Register(Payload payload)
    {
        if (payload.UserName == null || payload.Password == null)
        {
            return null;
        }
        
        var account = await _postgresDbContext.Accounts
            .FirstOrDefaultAsync(
                a => a.UserName == payload.UserName);

        if (account != null)
        {
            return null;
        }

        Account newAccount = new Account
        {
            ConnectionToken = new Guid(),
            UserName = payload.UserName,
            Password = payload.Password,
            Salt = RandomNumberGenerator.GetBytes(16),
        };

        newAccount.Password = Hash(payload.Password, newAccount.Salt);

        _postgresDbContext.Add(newAccount);
        await _postgresDbContext.SaveChangesAsync();
        
        
        return newAccount;
        
    }

    public async Task<Account> Login(Payload payload)
    {
        if (payload.UserName == null || payload.Password == null)
        {
            return null;
        }
        
        var account = await _postgresDbContext.Accounts
            .FirstOrDefaultAsync(
                a => a.UserName == payload.UserName);

        if (payload.UserName == null || payload.Password == null)
        {
            return null;
        }

        return Hash(payload.Password, account.Salt) == account.Password ? account : null;
        
    }

    public async Task<bool> DeleteUser(Guid id)
    {
        var verifyAccount = await _postgresDbContext.Accounts.FirstOrDefaultAsync(
            a => a.ConnectionToken == id);
        
        if (verifyAccount != null)
        {
            // On efface les images de dessus le cloud
            var pictures = await _postgresDbContext.Pictures
                .Where(p => p.AccountId == id)
                .ToArrayAsync();
            
            
            foreach (var picture in pictures)
            {
                var result = await _pictureService.DeletePhotoAsync(picture.PublicId);

                if (result.Result != "ok")
                {
                    return false;
                }
            }
            
            _postgresDbContext.Accounts.Remove(verifyAccount);
            await _postgresDbContext.SaveChangesAsync();
            
            return true;
        }

        return false;
    }

    
    private string Hash(string password, byte[] salt)
    {
        return (Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000,
            numBytesRequested: 32)));
    }
}