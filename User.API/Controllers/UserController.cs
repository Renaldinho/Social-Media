using Microsoft.AspNetCore.Mvc;
using User.API.DTOs;

namespace User.API.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{

    [HttpPut("{userId}")]
    public IActionResult EditUserProfile(string userId, [FromBody] UserProfileEditDTO userProfileEditDto)
    {
        // Mocked behavior: Log or return a message acknowledging the received changes without actual database operations
        return Ok(new { Message = $"Changes for user {userId} registered. Name: {userProfileEditDto.Name}, LastName: {userProfileEditDto.LastName}, Bio: {userProfileEditDto.Bio}" });
    }

    [HttpDelete("{userId}")]
    public IActionResult DeleteUser(string userId)
    {
        // Mocked behavior: Log or return a message acknowledging the user deletion without actual database operations
        return Ok(new { Message = $"User {userId} deletion registered." });
    }
    
    [HttpPost("create")]
    public IActionResult CreateUser([FromBody] CreateUserRequest request)
    {
        if (request.UserId == Guid.Empty)
        {
            return BadRequest("UserId is required");
        }
        // Here the intention is that we would create a user profile which would relate to instance in the auth service
        // And generate a random tag as their identifier for example sleepyUnicorn123, so that upon registration their profile is in the system
        // and can be viewed
        
        return Ok(new { Message = $"User with ID: {request.UserId} was successfully created." });
    }
}