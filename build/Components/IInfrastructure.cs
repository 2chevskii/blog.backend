using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Extensions;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Utilities;
using Serilog;

// ReSharper disable UnusedMember.Global

// ReSharper disable AllUnderscoreLocalParameterName

namespace Components;

interface IInfrastructure : INukeBuild, IHasSshClient, IHasScpClient
{
    [Parameter] AbsolutePath DeploymentPath => TryGetValue(() => DeploymentPath);

    AbsolutePath InfrastructureDirectory => DeploymentPath / "infrastructure";
    AbsolutePath DockerComposePath => InfrastructureDirectory / "docker-compose.yml";
    AbsolutePath MySqlDirectory => InfrastructureDirectory / "mysql";
    AbsolutePath MySqlInitDirectory => MySqlDirectory / "init.d";
    AbsolutePath MySqlDataDirectory => MySqlDirectory / "data";

    Target EnvDeploy => _ => _
        .Requires(() => DeploymentPath)
        .Executes(() =>
        {
            SshClient.EnsureConnected();
            ScpClient.EnsureConnected();

            CreateInfrastructureDirectories();
            DeployDockerCompose();
            DeployDatabaseInitScripts();
        });

    Target EnvStart => _ => _
        .DependsOn(EnvDeploy)
        .Executes(() =>
        {
            SshClient.EnsureConnected();

            SshClient.RunCommand($"docker compose -f {DockerComposePath:sn} up -d");
        });

    Target EnvStop => _ => _.Executes(() =>
    {
        SshClient.EnsureConnected();

        SshClient.RunCommand($"docker compose -f {DockerComposePath:sn} stop");
    });

    Target EnvTeardown => _ => _.Executes(() =>
    {
        SshClient.EnsureConnected();

        SshClient.RunCommand($"docker compose -f {DockerComposePath:sn} down");
    });

    void CreateInfrastructureDirectories()
    {
        List<AbsolutePath> directoriesToCreate =
            [InfrastructureDirectory, MySqlDirectory, MySqlInitDirectory, MySqlDataDirectory];

        Log.Information("CreateInfrastructureDirectories: {Paths}", directoriesToCreate);

        directoriesToCreate.ForEach(directory =>
        {
            var directoryExists = SshClient.RunCommand($"stat {directory:sn}").ExitStatus == 0;

            if (directoryExists)
            {
                Log.Information("Directory {Path:sn} exists", directory);
                return;
            }

            Log.Information("Creating directory {Path:sn}", directory);
            SshClient.RunCommand($"mkdir -p {directory:sn}");
        });
    }

    void DeployDockerCompose()
    {
        var localDockerComposePath = RootDirectory / "docker" / "docker-compose.yml";
        var localDockerComposeMd5 = localDockerComposePath.GetFileHash();

        var remoteDockerComposeExists = SshClient.RunCommand($"stat {DockerComposePath:sn}").ExitStatus == 0;

        if (remoteDockerComposeExists)
        {
            Log.Information("Remote docker compose file exists");

            var remoteDockerComposeMd5 =
                SshClient.RunCommand($"md5sum {DockerComposePath:sn}")
                    .Result
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .First();

            Log.Information(
                "Local docker compose MD5: {LocalHash} Remote docker compose MD5: {RemoteHash}",
                localDockerComposeMd5,
                remoteDockerComposeMd5
            );

            if (localDockerComposeMd5.Equals(remoteDockerComposeMd5, StringComparison.OrdinalIgnoreCase))
            {
                Log.Information("Local and remote docker compose hashes match");
                return;
            }

            Log.Information("Local and remote docker compose hashes do not match");

            Log.Information("Removing outdated docker compose at {Target}", DockerComposePath);
            SshClient.RunCommand($"rm {DockerComposePath:sn}");
        }

        Log.Information("Uploading docker compose from {Src} to {Target}", localDockerComposePath, DockerComposePath);
        ScpClient.Upload(localDockerComposePath.ToFileInfo(), DockerComposePath);
    }

    void DeployDatabaseInitScripts()
    {
        var localInitScriptsDir = RootDirectory / "scripts" / "mysql";
        var initScripts = localInitScriptsDir.GetFiles().ToList();

        Log.Information("Found {Count} database init scripts", initScripts.Count);

        initScripts.ForEach(script =>
        {
            var content = script.ReadAllText();

            var contentRendered = content.ReplaceRegex(@"\$([A-Z0-9_]+)", eval =>
            {
                var varName = eval.Groups[1].Value;

                var varValue = EnvironmentInfo.GetVariable(varName);

                return varValue;
            });

            var targetPath = MySqlInitDirectory / script.Name;

            Log.Information("Deploying script {Src} to {Target}", script, targetPath);
            Log.Information("Script contents:\n{Content}", contentRendered);

            using var contentStream = new MemoryStream(Encoding.UTF8.GetBytes(contentRendered), writable: false);

            ScpClient.Upload(contentStream, targetPath);
        });
    }
}
