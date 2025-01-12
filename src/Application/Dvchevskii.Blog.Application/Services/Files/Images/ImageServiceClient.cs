using System.Net.Http.Json;
using Dvchevskii.Blog.Application.Contracts.Services.Files.Images;
using Microsoft.Extensions.Options;

namespace Dvchevskii.Blog.Application.Services.Files.Images;

internal class ImageServiceClient(IOptions<ImageServiceClientOptions> options) : IImageServiceClient
{
    private ImageServiceClientOptions Options => options.Value;

    public async Task<Uri> GetUrl(Guid id)
    {
        var httpClient = new HttpClient
        {
            BaseAddress = Options.Url
        };

        var response = await httpClient.GetFromJsonAsync<Uri>($"/images/{id}/url");

        return response ?? throw new Exception();
    }

    public async Task<Dictionary<Guid, Uri>> GetUrls(IEnumerable<Guid> ids)
    {
        var httpClient = new HttpClient { BaseAddress = Options.Url };

        var response = await httpClient.PostAsJsonAsync("images/url-list", ids);
        var urls = await response.Content.ReadFromJsonAsync<Dictionary<Guid, Uri>>();
        if (urls == null)
        {
            throw new Exception();
        }

        return urls;
    }
}
