using System.Net;
using System.Net.Http.Json;
using Dvchevskii.Blog.Shared.Contracts.Assets.Images;
using Microsoft.Extensions.Http;

namespace Dvchevskii.Blog.Shared.Assets.Images;

internal class ImageAssetApiClient(IHttpClientFactory httpClientFactory)
{
    public async Task<ImageAssetDto?> GetImageAsset(Guid id)
    {
        try
        {
            var asset = await httpClientFactory.CreateClient(nameof(ImageAssetApiClient))
                .GetFromJsonAsync<ImageAssetDto>($"images/{id}");
            return asset;
        }
        catch (HttpRequestException exception)
        {
            if (exception.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            throw;
        }
    }

    public async Task<Uri?> GetUrl(Guid id)
    {
        try
        {
            var strUrl = await httpClientFactory.CreateClient(nameof(ImageAssetApiClient))
                .GetFromJsonAsync<string>($"images/{id}/url");
            return new Uri(strUrl!);
        }
        catch (HttpRequestException exception)
        {
            if (exception.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            throw;
        }
    }
}
