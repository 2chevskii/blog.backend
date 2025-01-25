using System.IO;
using System.Text;
using Dvchevskii.Blog.Build.Components;
using Renci.SshNet;
using Serilog;

namespace Dvchevskii.Blog.Build;

class Build : NukeBuild, IInfrastructure, ICompile
{
    [Parameter] readonly string DeploymentHost;
    [Parameter] readonly int DeploymentSshPort;
    [Parameter] readonly string DeploymentSshUser;
    [Parameter] readonly string DeploymentSshPrivateKey;
    [Parameter] readonly string GithubEnvironmentName;

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    public SshClient SshClient { get; private set; }
    public ScpClient ScpClient { get; private set; }


    public static int Main() => Execute<Build>();

    protected override void OnBuildInitialized()
    {
        if (IsLocalBuild)
        {
            return;
        }

        Log.Information("Initializing SSH and SCP clients for {User}@{Host}:{Port}",
            DeploymentSshUser,
            DeploymentHost,
            DeploymentSshPort
        );

        var privateKey = new PrivateKeyFile(
            new MemoryStream(Encoding.ASCII.GetBytes(DeploymentSshPrivateKey))
        );

        SshClient = new SshClient(
            new PrivateKeyConnectionInfo(
                DeploymentHost,
                DeploymentSshPort,
                DeploymentSshUser,
                privateKey
            )
        );
        ScpClient = new ScpClient(
            new PrivateKeyConnectionInfo(
                DeploymentHost,
                DeploymentSshPort,
                DeploymentSshUser,
                privateKey
            )
        );
    }

    protected override void OnBuildFinished()
    {
        if (IsLocalBuild)
        {
            return;
        }

        SshClient.Dispose();
        ScpClient.Dispose();
    }
}
