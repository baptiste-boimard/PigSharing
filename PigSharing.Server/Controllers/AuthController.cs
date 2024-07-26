using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PigSharing.Client.Models;
using PigSharing.Server.Database.Models;
using PigSharing.Server.Repositories;

namespace PigSharing.Server.Controller;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AuthRepository _authRepository;

    public AuthController (AuthRepository authRepository)
    {
        _authRepository = authRepository;
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] Payload payload)
    {
        Account account = await _authRepository.Register(payload);

        if (account == null)
        {
            return Forbid();
        }
        
        return Ok(account);
    }
    
    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] Payload payload)
    {
        Account account = await _authRepository.Login(payload);

        if (account == null)
        {
            return Forbid();
        }
        
        return Ok(account);
    }
    
    [HttpDelete]
    [Route("deleteuser")]
    public async Task<IActionResult> DeleteUser([FromBody] Guid id)
    {
        var result = await _authRepository.DeleteUser(id);
        
        if (result == false)
        {
            return Forbid();
        }
        
        return Ok(result);
    }
    
    [HttpPost]
    [Route("upload")]
    public async Task<IActionResult> Upload([FromBody] IFormFile file)
    {
        Console.WriteLine("dfdf");
        // Methode de iservice
        return Ok();
    }
}