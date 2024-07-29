using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;

namespace PigSharing.Server.Service;

public class PictureService
{
    private readonly Cloudinary _cloudinary;
    
    public PictureService(IOptions<CloudinarySettings> config)
    {
        var acc = new Account(
            config.Value.Cloudname,
            config.Value.ApiKey,
            config.Value.ApiSecret
        );

        _cloudinary = new Cloudinary(acc);
    }
    
    public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
    {
        try
        {
            var uploadResult = new ImageUploadResult();
            if (file.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    memoryStream.Position = 0;
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.FileName, memoryStream),
                        // Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face")
                    };
                    uploadResult = await _cloudinary.UploadAsync(uploadParams);
                }
            }
            return uploadResult;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
       
    }

    public async Task<DeletionResult> DeletePhotoAsync(string publicId)
    {
        var deleteParams = new DeletionParams(publicId);
        var result = await _cloudinary.DestroyAsync(deleteParams);
        return result;
    }
    
}