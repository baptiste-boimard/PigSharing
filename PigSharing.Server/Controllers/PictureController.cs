using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using PigSharing.Server.Database;
using PigSharing.Server.Repositories;
using PigSharing.Server.Service;
using PigSharing.Share.Models;

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
    public async Task<IActionResult> Upload([FromForm]IFormFile file, [FromForm]string account)
    {
        
        var resultUrl = await _pictureService.AddPhotoAsync(file);

        var accountDes = JsonSerializer.Deserialize<Account>(account);
        
        var response = await _pictureRepository.Upload(resultUrl, accountDes);
        return Ok();
    }
    
    [HttpGet]
    [Route("getallpublics")]
    public async Task<IActionResult> GetAllPublic()
    {

        try
        {
            var response = _pictureRepository.GetAllPublics();

            var result = response.Result;
            
            return Ok(response.Result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpPut]
    [Route("updatestatus")]
    public async Task<IActionResult> UpdateStatusPrivate([FromBody] Picture picture)
    {
        if (picture == null || picture.Id == Guid.Empty)
        {
            return BadRequest("Invalid picture data.");
        }

        Console.WriteLine($"Received picture: Id = {picture.Id}, Private = {picture.Private}");
        
        var response = await _pictureRepository.UpdateStatusPrivate(picture);

        return Ok(response);
    }
}