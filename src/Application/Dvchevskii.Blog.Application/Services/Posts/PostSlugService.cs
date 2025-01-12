using Dvchevskii.Blog.Application.Contracts.Services.Posts;
using Dvchevskii.Blog.Application.Utilities.Posts;
using Dvchevskii.Blog.Core.Repositories.Posts;

namespace Dvchevskii.Blog.Application.Services.Posts;

internal class PostSlugService(IPostRepository postRepository) : IPostSlugService
{
    public async Task<string> GetAvailableSlug(string title)
    {
        var wantsSlug = SlugUtility.WriteAsSlug(title);

        var existingSlugs = await postRepository.GetSlugsStartingWith(wantsSlug);

        if (existingSlugs.Count == 0)
        {
            return wantsSlug;
        }

        var number = FindBiggestNumber(existingSlugs) + 1;
        return $"{wantsSlug}-{number}";
    }

    private int FindBiggestNumber(List<string> slugs)
    {
        return slugs.Select(slug =>
        {
            var numberStartsAt = -1;
            for (var i = slug.Length - 1; i >= 0; i--)
            {
                var c = slug[i];
                if (!char.IsDigit(c))
                {
                    numberStartsAt = i + 1;
                    break;
                }
            }

            if (numberStartsAt == -1)
            {
                return 0;
            }

            var numberPart = slug.AsSpan().Slice(numberStartsAt);
            var parsedNumber = int.Parse(numberPart);
            return parsedNumber;
        }).Max();
    }
}
