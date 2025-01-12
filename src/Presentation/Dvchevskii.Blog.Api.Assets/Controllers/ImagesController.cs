using Dvchevskii.Blog.Application.Contracts.Services.Files.Images;
using Dvchevskii.Blog.Assets.Contracts.Entities.Files.Images;
using Dvchevskii.Blog.Assets.Contracts.Services.Files.Images;
using Microsoft.AspNetCore.Mvc;

namespace Dvchevskii.Blog.Assets.Controllers;

[ApiController]
[Route("[controller]")]
public class ImagesController(IImageService imageService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> UploadImage(IFormFile image)
    {
        await using var stream = image.OpenReadStream();
        var img = await imageService.Upload(new UploadImageDto
        {
            Data = stream,
            ContentType = image.ContentType
        });

        var url = await imageService.GetUrl(img.Id);

        return Created(url, img);
    }

    [HttpGet("{id}/url")]
    public async Task<ActionResult<Uri>> GetImageUrl(Guid id)
    {
        var url = await imageService.GetUrl(id);
        return url;
    }

    [HttpPost("url-list")]
    public async Task<Dictionary<Guid, Uri>> GetImageUrlList([FromBody] IEnumerable<Guid> ids)
    {
        var result = await imageService.GetUrls(ids);
        return result;
    }
}
