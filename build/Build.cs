using System.IO;
using System.Text;
using System.Threading;
using Nuke.Common;
using Nuke.Common.CI.GitHubActions;
using Renci.SshNet;
using Serilog;

class Build : NukeBuild
{
    [Parameter] readonly string DeploymentHost;
    [Parameter] readonly int DeploymentSshPort;
    [Parameter] readonly string DeploymentSshUser;
    [Parameter] readonly string DeploymentSshPrivateKey;
    [Parameter] readonly string DeploymentPath;
    [Parameter] readonly string GithubEnvironmentName;

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    string DockerComposeFilePath => DeploymentPath + "/docker-compose.yml";

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

    Target EnvironmentStart => _ => _.Executes(() =>
    {
    });

    Target EnvironmentShutdown => _ => _.Executes(async () =>
    {
        Log.Information("Shutting down environment {EnvironmentName}...", GithubEnvironmentName);

        using var sshClient = CreateSshClient();

        await sshClient.ConnectAsync(CancellationToken.None);

        Log.Information("Running docker compose stop");
        sshClient.RunCommand(
            $"docker compose -f {DockerComposeFilePath} stop -t 15"
        );
    });

    public static int Main() => Execute<Build>(x => x.Compile);

    private SshClient CreateSshClient()
    {
        var client = new SshClient(
            new PrivateKeyConnectionInfo(
                DeploymentHost,
                DeploymentSshPort,
                DeploymentSshUser,
                CreatePrivateKey()
            )
        );

        return client;
    }

    private ScpClient CreateScpClient()
    {
        var client = new ScpClient(
            new PrivateKeyConnectionInfo(
                DeploymentHost,
                DeploymentSshPort,
                DeploymentSshUser,
                CreatePrivateKey()
            )
        );

        return client;
    }

    private PrivateKeyFile CreatePrivateKey() =>
        new PrivateKeyFile(new MemoryStream(Encoding.ASCII.GetBytes(DeploymentSshPrivateKey)));
}
