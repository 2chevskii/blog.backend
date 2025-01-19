using Dvchevskii.Blog.Application.Contracts.Entities.Posts;
using Dvchevskii.Blog.Application.Contracts.Services.Posts;
using Dvchevskii.Blog.Application.Contracts.ValueObjects.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace Dvchevskii.Blog.Api.Reader.Controllers;

[ApiController]
[Route("[controller]")]
public class PostsController(IPostReaderService postReaderService) : ControllerBase
{
    [HttpGet]
    public async Task<LimitedQueryResult<PostFeedEntryDto>> GetFeed(int offset = 0, int limit = 0)
    {
        var posts = await postReaderService.GetFeed(new LimitedQuerySettings(offset, limit));
        return posts;
    }

    [HttpGet("{slug}")]
    public async Task<IActionResult> GetPost(string slug)
    {
        var post = await postReaderService.Get(slug);
        return Ok(post);
    }
}
