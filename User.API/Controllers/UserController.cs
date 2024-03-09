using Microsoft.AspNetCore.Mvc;

namespace User.API.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Request received at UserController.");
    }
}