using Dvchevskii.Blog.Api.Admin.Models;
using Dvchevskii.Blog.Application.Contracts.Entities.Authentication.Users;
using Dvchevskii.Blog.Application.Contracts.Entities.Posts;
using Dvchevskii.Blog.Application.Contracts.Services.Authentication.Users;
using Dvchevskii.Blog.Application.Contracts.Services.Files.Images;
using Dvchevskii.Blog.Application.Contracts.Services.Posts;
using Dvchevskii.Blog.Application.Contracts.ValueObjects.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace Dvchevskii.Blog.Api.Admin.Controllers;

[ApiController]
[Route("[controller]")]
public class PostsController(
    IPostService postService,
    IUserService userService,
    IImageServiceClient imageServiceClient,
    IPostAdminService postAdminService
) : ControllerBase
{
    [HttpGet]
    public async Task<LimitedQueryResult<PostInfoModel>> GetPostList(
        int offset = 0,
        int limit = 0,
        [FromQuery(Name = "published_only")] bool onlyPublished = false
    )
    {
        var settings = new LimitedQuerySettings(offset, limit);
        var list = await postAdminService.GetInfoList(settings, onlyPublished);
        return list.Map(dto => new PostInfoModel
        {
            Id = dto.Id,
            Slug = dto.Slug,
            Title = dto.Title,
            IsPublished = dto.IsPublished,
            CreatedBy = new PostEditorModel
            {
                Id = dto.CreatedBy.Id,
                Username = dto.CreatedBy.Username,
                Timestamp = dto.CreatedBy.Timestamp,
            },
            UpdatedBy = dto.UpdatedBy == null
                ? null
                : new PostEditorModel
                {
                    Id = dto.UpdatedBy.Id,
                    Username = dto.UpdatedBy.Username,
                    Timestamp = dto.UpdatedBy.Timestamp,
                },
        });
    }

    [HttpGet("{id}")]
    public async Task<PostEditModel> GetForEdit(Guid id)
    {
        var post = await postService.Get(id);
        var createdByUser = await userService.Get(post.CreatedBy);
        var createdByInfo = new PostEditorModel
        {
            Id = createdByUser.Id,
            Username = createdByUser.Username,
            Timestamp = post.CreatedAt,
        };

        PostEditorModel? updatedByInfo = null;
        if (post.UpdatedBy.HasValue)
        {
            var updatedByUser = await userService.Get(post.UpdatedBy.Value);
            updatedByInfo = new PostEditorModel
            {
                Id = updatedByUser.Id,
                Username = updatedByUser.Username,
                Timestamp = post.UpdatedAt ?? throw new Exception(),
            };
        }

        Uri? headerImageUrl = null;
        if (post.HeaderImageId.HasValue)
        {
            headerImageUrl = await imageServiceClient.GetUrl(post.HeaderImageId.Value);
        }

        var model = new PostEditModel(
            post.Id,
            post.Slug,
            post.Title,
            null,
            post.Body,
            post.IsPublished,
            createdByInfo,
            updatedByInfo,
            post.HeaderImageId,
            headerImageUrl
        );

        return model;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreatePostRequest request)
    {
        var post = await postService.Create(new CreatePostDto
        {
            Title = request.Title,
            Body = request.Body,
            IsPublished = request.IsPublished,
            HeaderImageId = request.HeaderImageId,
        });

        return Ok(post);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdatePostRequest request)
    {
        if (id != request.Id)
        {
            throw new Exception("ID mismatch");
        }

        var post = await postService.Update(
            new UpdatePostDto
            {
                Id = id,
                Title = request.Title,
                Body = request.Body,
                IsPublished = request.IsPublished,
                HeaderImageId = request.HeaderImageId,
            }
        );
        return Ok(post);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        /*await postService.Delete(id);
        return Ok();*/

        throw new NotImplementedException();
    }
}
