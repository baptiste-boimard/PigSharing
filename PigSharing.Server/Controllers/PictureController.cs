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
        
        var result = await _pictureService.AddPhotoAsync(file);
        
        var response = await _pictureRepository.Upload(result.Url.ToString(), result.PublicId, JsonSerializer.Deserialize<Account>(account));
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

    [HttpGet]
    [Route("getallimages/{idAccount}")]
    public async Task<IActionResult> GetAllImages(Guid idAccount)
    {
        try
        {
            var response = _pictureRepository.GetAllImages(idAccount);

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

    [HttpPost]
    [Route("deleteimage")]
    public async Task<IActionResult> DeleteImage([FromBody] Picture picture)
    {
        var response = await _pictureRepository.DeleteImage(picture);

        if (response)
        {
           var result = await _pictureService.DeletePhotoAsync(picture.PublicId);
           
           if (result.Result != "ok")
           {
               return Forbid();
           }
        }
        
        return Ok();
    }
}