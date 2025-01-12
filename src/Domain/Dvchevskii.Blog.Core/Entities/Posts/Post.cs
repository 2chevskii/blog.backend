using System.Text.RegularExpressions;
using Dvchevskii.Blog.Application.Contracts.Entities.Posts;
using Dvchevskii.Blog.Core.Entities.Common;
using Dvchevskii.Blog.Core.Entities.Files;
using Dvchevskii.Blog.Core.Entities.Files.Images;

namespace Dvchevskii.Blog.Core.Entities.Posts;

public sealed class Post : Entity
{
    private static readonly Regex SlugBannedCharactersRegex = new Regex("[^a-z0-9-]");

    public required string Slug { get; set; }
    public required string Title { get; set; }
    public required string? Tagline { get; set; }
    public required string Body { get; set; }
    public required bool IsPublished { get; set; }

    public required Guid? HeaderImageId { get; set; }

    public Image? HeaderImage { get; set; }

    public static Post Create(CreatePostDto dto)
    {
        ValidateSlugOrThrow(dto.Slug);
        ValidateTitleOrThrow(dto.Title);

        return new Post
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            Tagline = null,
            Body = dto.Body,
            Slug = dto.Slug,
            IsPublished = dto.IsPublished,
            HeaderImageId = dto.HeaderImageId,
        };
    }

    public Post Update(UpdatePostDto dto)
    {
        ValidateSlugOrThrow(dto.Slug);
        ValidateTitleOrThrow(dto.Title);

        Slug = dto.Slug;
        Title = dto.Title;
        Body = dto.Body;
        IsPublished = dto.IsPublished;
        HeaderImageId = dto.HeaderImageId;

        return this;
    }

    private static void ValidateSlugOrThrow(string slug)
    {
        if (SlugBannedCharactersRegex.IsMatch(slug))
        {
            throw new ArgumentException("Slug contains banned characters", nameof(slug));
        }
    }

    private static void ValidateTitleOrThrow(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Title should not be empty", nameof(title));
        }
    }
}
