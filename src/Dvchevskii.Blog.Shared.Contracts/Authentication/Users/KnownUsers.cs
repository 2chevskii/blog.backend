namespace Dvchevskii.Blog.Shared.Contracts.Authentication.Users;

public static class KnownUsers
{
    public static readonly KnownUserInfo System =
        new KnownUserInfo(Guid.Parse("00000000-0000-0000-0000-000000000001"), true, "system");

    [KnownUserPassword("debugAdminPwd")] public static readonly KnownUserInfo DebugAdmin =
        new KnownUserInfo(Guid.Parse("00000000-0000-0000-0000-000000000002"), true, "debug_admin");

    [KnownUserPassword("debugUsrPwd")] public static readonly KnownUserInfo DebugUser =
        new KnownUserInfo(Guid.Parse("00000000-0000-0000-0000-000000000003"), false, "debug_user");

    [AttributeUsage(AttributeTargets.Field)]
    public class KnownUserPasswordAttribute(string password) : Attribute
    {
        public string Password { get; init; } = password;
    }

    public readonly record struct KnownUserInfo(Guid Id, bool IsAdmin, string? Username)
    {
        public AuthenticationData ToAuthenticationData()
        {
            return new AuthenticationData
            {
                UserId = Id,
                IsAdmin = IsAdmin,
                Username = Username,
            };
        }
    }
}
