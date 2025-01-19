using Renci.SshNet;

namespace Components;

interface IHasScpClient
{
    ScpClient ScpClient { get; }
}