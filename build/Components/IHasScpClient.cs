using Renci.SshNet;

namespace Dvchevskii.Blog.Build.Components;

interface IHasScpClient
{
    ScpClient ScpClient { get; }
}
