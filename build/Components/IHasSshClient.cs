using Renci.SshNet;

namespace Components;

interface IHasSshClient
{
    SshClient SshClient { get; }
}
