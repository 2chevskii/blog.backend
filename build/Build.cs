using System.IO;
using System.Text;
using Components;
using Nuke.Common;
using Renci.SshNet;
using Serilog;

class Build : NukeBuild, IInfrastructure
{
    [Parameter] readonly string DeploymentHost;
    [Parameter] readonly int DeploymentSshPort;
    [Parameter] readonly string DeploymentSshUser;
    [Parameter] readonly string DeploymentSshPrivateKey;
    [Parameter] readonly string DeploymentPath;
    [Parameter] readonly string GithubEnvironmentName;

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    public SshClient SshClient { get; private set; }
    public ScpClient ScpClient { get; private set; }

    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
        });

    Target Restore => _ => _
        .Executes(() =>
        {
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
        });

    public static int Main() => Execute<Build>(x => x.Compile);

    protected override void OnBuildInitialized()
    {
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
        SshClient.Dispose();
        ScpClient.Dispose();
    }
}
