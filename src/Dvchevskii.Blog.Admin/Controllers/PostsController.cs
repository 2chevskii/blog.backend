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
}
