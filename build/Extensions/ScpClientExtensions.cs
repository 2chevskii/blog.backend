using Renci.SshNet;

namespace Dvchevskii.Blog.Build.Extensions;

static class ScpClientExtensions
{
    public static void EnsureConnected(this ScpClient scpClient)
    {
        if (!scpClient.IsConnected) scpClient.Connect();
    }
}
