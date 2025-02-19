﻿using System.Text.RegularExpressions;

namespace Dvchevskii.Blog.Application.Utilities.Posts;

public static class SlugUtility
{
    public static string WriteAsSlug(string input)
    {
        var lower = input.ToLowerInvariant();
        var replaced = Regex.Replace(lower, "[^a-z0-9-]", "-");
        var replaced2 = Regex.Replace(replaced, "-{2,}", "-");
        var replaced3 = Regex.Replace(replaced2, "^-|-$", "");

        return replaced3;
    }
}
