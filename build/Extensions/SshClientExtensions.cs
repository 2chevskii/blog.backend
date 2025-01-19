using System.Threading;
using System.Threading.Tasks;
using Renci.SshNet;

namespace Extensions;

static class SshClientExtensions
{
    public static void EnsureConnected(this SshClient sshClient)
    {
        if (!sshClient.IsConnected) sshClient.Connect();
    }
}
