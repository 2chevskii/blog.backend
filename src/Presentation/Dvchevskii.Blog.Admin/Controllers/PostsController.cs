using Dvchevskii.Blog.Admin.Models;
using Dvchevskii.Blog.Admin.Services;
using Dvchevskii.Blog.Shared.Contracts.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace Dvchevskii.Blog.Admin.Controllers;

[ApiController]
[Route("[controller]")]
internal class PostsController(PostService postService) : ControllerBase
{
    [HttpGet]
    public async Task<PaginationQueryResult<PostInfoModel>> GetPostList(
        int offset,
        int limit,
        [FromQuery(Name = "include_non_published")]
        bool includeNonPublished
    )
    {
        var result = await postService.GetInfoList(offset, limit, includeNonPublished);

        return result;
    }

    [HttpGet("{id}")]
    public async Task<PostEditModel> GetForEdit(Guid id)
    {
        var model = await postService.GetForEdit(id);

        return model;
    }

    [HttpPost]
    public async Task<PostEditModel> Create(CreatePostRequest request)
    {
        var post = await postService.Create(request);
        return post;
    }

    [HttpPut("{id}")]
    public async Task<PostEditModel> Update(Guid id, UpdatePostRequest request)
    {
        if (id != request.Id)
        {
            throw new Exception("ID mismatch");
        }

        var post = await postService.Update(request);
        return post;
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await postService.Delete(id);
        return Ok();
    }
}
