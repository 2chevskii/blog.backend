namespace Dvchevskii.Blog.Auth.Models;

public sealed record LocalSignUpRequest(string Username, string? Email, string Password);
