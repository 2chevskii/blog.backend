using Dvchevskii.Blog.Assets.Services;
using Dvchevskii.Blog.Shared.Contracts.Assets.Images;
using Microsoft.AspNetCore.Mvc;

namespace Dvchevskii.Blog.Assets.Controllers;

[ApiController]
[Route("[controller]")]
internal class ImagesController(ImageService imageService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> UploadImage(IFormFile image)
    {
        await using var stream = image.OpenReadStream();
        var img = await imageService.Create(stream, image.ContentType);

        var url = await imageService.GetPreSignedUrl(img.Id);

        return Created(url, img);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ImageAssetDto>> GetImageAsset(Guid id)
    {
        var img = await imageService.Find(id);

        if (img == null)
        {
            return NotFound();
        }

        return img;
    }

    [HttpGet("{id}/url")]
    public async Task<ActionResult<Uri>> GetImageUrl(Guid id)
    {
        var url = await imageService.GetPreSignedUrl(id);

        if (url == null)
        {
            return NotFound();
        }

        return url;
    }

    [HttpPost("url-list")]
    public async Task<Dictionary<Guid, Uri>> GetImageUrlList([FromBody] Guid[] ids)
    {
        var result = await imageService.GetPreSignedUrlList(ids);

        return result;
    }
}
