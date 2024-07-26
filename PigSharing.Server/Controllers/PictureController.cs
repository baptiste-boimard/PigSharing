using Microsoft.AspNetCore.Mvc;
using PigSharing.Server.Database;
using PigSharing.Server.Repositories;
using PigSharing.Server.Service;

namespace PigSharing.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PictureController : ControllerBase
{
    private readonly PictureRepository _pictureRepository;
    private readonly PictureService _pictureService;

    public PictureController(PictureRepository pictureRepository, PictureService pictureService)
    {
        _pictureRepository = pictureRepository;
        _pictureService = pictureService;
    }

    [HttpPost]
    [Route("upload")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        // var response = _pictureRepository.Upload(file);
        
        var resultUrl = await _pictureService.AddPhotoAsync(file);

        var response = await _pictureRepository.Upload(resultUrl);
        return Ok();
    }
    
    
    
    
}