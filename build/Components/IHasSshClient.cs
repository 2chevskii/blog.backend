using Renci.SshNet;

namespace Dvchevskii.Blog.Build.Components;

interface IHasSshClient
{
    SshClient SshClient { get; }
}
