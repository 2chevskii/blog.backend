using System.Threading;
using System.Threading.Tasks;
using Renci.SshNet;

namespace Extensions;

static class ScpClientExtensions
{
    public static void EnsureConnected(this ScpClient scpClient)
    {
        if (!scpClient.IsConnected) scpClient.Connect();
    }
}
