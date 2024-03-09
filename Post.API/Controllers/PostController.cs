using Microsoft.AspNetCore.Mvc;
using Post.API.DTOs;

namespace Post.API.Controllers;

[ApiController]
[Route("[controller]")]
public class PostsController : ControllerBase
{
    [HttpGet("{postId}")]
    public IActionResult GetPost(int postId)
    {
        // Mocked behavior: Return a dummy post
        return Ok(new PostDTO 
        { 
            PostId = postId, 
            Title = "Sample Post Title", 
            Content = "This is a sample post content." 
        });
    }

    [HttpPost]
    public IActionResult CreatePost([FromBody] PostCreateDTO createDTO)
    {
        // Mocked behavior: return a message acknowledging the post creation
        return Ok(new { Message = $"Post created with Title: {createDTO.Title}" });
    }

    [HttpPut("{postId}")]
    public IActionResult UpdatePost(Guid postId, [FromBody] PostUpdateDTO updateDTO)
    {
        // Mocked behavior:return a message acknowledging the post update
        return Ok(new { Message = $"Post {postId} updated. New Title: {updateDTO.Title}, New Content: {updateDTO.Content}" });
    }

    [HttpDelete("{postId}")]
    public IActionResult DeletePost(int postId)
    {
        // Mocked behavior: return a message acknowledging the post deletion
        return Ok(new { Message = $"Post {postId} deleted." });
    }
}