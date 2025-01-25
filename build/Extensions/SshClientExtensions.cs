using Renci.SshNet;

namespace Dvchevskii.Blog.Build.Extensions;

static class SshClientExtensions
{
    public static void EnsureConnected(this SshClient sshClient)
    {
        if (!sshClient.IsConnected) sshClient.Connect();
    }
}
