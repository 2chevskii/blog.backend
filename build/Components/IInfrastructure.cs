using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Dvchevskii.Blog.Build.Extensions;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Utilities;
using Serilog;

namespace Dvchevskii.Blog.Build.Components;

interface IInfrastructure : IHasSshClient, IHasScpClient, IHasRepositoryFiles, IHasDeploymentFiles
{
    [Parameter] string MySqlRootPasswd => TryGetValue(() => MySqlRootPasswd);
    [Parameter] string MySqlBackendPasswd => TryGetValue(() => MySqlBackendPasswd);

    Target EnvDeploy => _ => _
        .Requires(() => DeploymentPath)
        .OnlyWhenStatic(() => IsServerBuild)
        .Executes(() =>
        {
            CreateRequiredDirectories();
            DeployDockerCompose();
            DeployDatabaseInitScripts();
        });

    Target EnvStart => _ => _
        .TryDependsOn<IInfrastructure>(c => c.EnvDeploy)
        .Executes(() =>
        {
            // Use SSH commands for remote builds
            if (IsLocalBuild)
            {
                return;
            }

            RunSshCommandAndLog(GetDockerComposeCommand("up -d"));
        }, () =>
        {
            // Use local commands for local builds
            if (IsServerBuild)
            {
                return;
            }

            ProcessTasks.StartProcess("docker", $"compose -f {RepositoryFiles.DockerComposeLocal:sn} up -d")
                .AssertZeroExitCode();
        });

    Target EnvStop => _ => _.Executes(() =>
    {
        if (IsLocalBuild)
        {
            return;
        }

        RunSshCommandAndLog(GetDockerComposeCommand("stop"));
    }, () =>
    {
        if (IsServerBuild)
        {
            return;
        }

        ProcessTasks.StartProcess("docker", $"compose -f {RepositoryFiles.DockerComposeLocal:sn} stop")
            .AssertZeroExitCode();
    });

    Target EnvTeardown => _ => _.Executes(() =>
    {
        if (IsLocalBuild)
        {
            return;
        }

        RunSshCommandAndLog(GetDockerComposeCommand("down"));
    }, () =>
    {
        if (IsServerBuild)
        {
            return;
        }

        ProcessTasks.StartProcess("docker", $"compose -f {RepositoryFiles.DockerComposeLocal:sn} down")
            .AssertZeroExitCode();
    });

    private void CreateRequiredDirectories()
    {
        foreach (var path in DeploymentFiles.RequiredDirectories)
        {
            if (CheckIfRemoteFileExists(path))
            {
                continue;
            }

            RunSshCommandAndLog($"mkdir -p {path:sn}");
        }
    }

    void DeployDockerCompose()
    {
        if (CheckIfRemoteFileExists(DeploymentFiles.DockerCompose))
        {
            if (CheckIfRemoteFileMatchesMd5(DeploymentFiles.DockerCompose, RepositoryFiles.DockerCompose.GetFileHash()))
            {
                return;
            }

            RunSshCommandAndLog($"rm {DeploymentFiles.DockerCompose:sn}");
        }

        UploadFileThroughScpAndLog(RepositoryFiles.DockerCompose, DeploymentFiles.DockerCompose);
    }

    void DeployDatabaseInitScripts()
    {
        foreach (var script in RepositoryFiles.MySqlInitScripts)
        {
            var scriptText = RenderDbInitScript(script.ReadAllText());

            var targetPath = DeploymentFiles.GetMySqlInitScriptPath(script.ToFileInfo());

            if (CheckIfRemoteFileExists(targetPath))
            {
                if (CheckIfRemoteFileMatchesMd5(targetPath, scriptText.GetMD5Hash()))
                {
                    continue;
                }

                RunSshCommandAndLog($"rm {targetPath:sn}");
            }

            using var scriptTextStream = new MemoryStream(
                Encoding.UTF8.GetBytes(scriptText),
                writable: false
            );
            UploadFileThroughScpAndLog(scriptTextStream, targetPath);
        }
    }

    #region Utility

    void UploadFileThroughScpAndLog(AbsolutePath source, AbsolutePath target)
    {
        ScpClient.EnsureConnected();
        ScpClient.Upload(source.ToFileInfo(), target);
        Log.Information("Uploaded file {Source} to {Target}", source, target);
    }

    void UploadFileThroughScpAndLog(Stream source, AbsolutePath target)
    {
        ScpClient.EnsureConnected();
        ScpClient.Upload(source, target);
        Log.Information("Uploaded file from stream ({StreamLength} bytes) to {Target}", source.Length, target);
    }

    void RunSshCommandAndLog(string command)
    {
        Log.Verbose("Running SSH command: {Command}", command);
        SshClient.EnsureConnected();
        var sshCommand = SshClient.RunCommand(command);
        Log.Information("SSH command {Command} output ({ExitCode}): {Output}",
            command,
            sshCommand.ExitStatus,
            sshCommand.Result
        );
    }

    string GetDockerComposeEnvVars()
    {
        var envVars = new Dictionary<string, string>
        {
            { "MYSQL_ROOT_PASSWD", MySqlRootPasswd.SingleQuoteIfNeeded() },
            { "MYSQL_INIT_SCRIPTS_DIR", DeploymentFiles.MySqlInitDbDirectory.ToString("sn") },
            { "MYSQL_DATA_DIR", DeploymentFiles.MySqlDataDirectory.ToString("sn") }
        };

        return envVars.Select(pair => $"{pair.Key}={pair.Value}")
            .JoinSpace();
    }

    string GetDockerComposeCommand(string command)
    {
        var envVars = GetDockerComposeEnvVars();
        var commandBase = GetDockerComposeCommandBase();

        return $"{envVars} {commandBase} {command}";
    }

    string GetDockerComposeCommandBase() => $"docker compose -f {DeploymentFiles.DockerCompose:sn}";


    bool CheckIfRemoteFileExists(AbsolutePath path)
    {
        Log.Verbose("Checking if remote file {Path} exists", path);

        SshClient.EnsureConnected();
        var exists = SshClient.RunCommand($"stat {path:sn}").ExitStatus == 0;

        Log.Information("Remote file {Path} {Exists}", path, exists ? "exists" : "does not exist");

        return exists;
    }

    bool CheckIfRemoteFileMatchesMd5(AbsolutePath path, string md5)
    {
        Log.Verbose("Checking if remote file {Path} matches md5 {Md5}", path, md5);

        SshClient.EnsureConnected();
        var remoteFileMd5 = SshClient.RunCommand($"md5sum {path:sn}")
            .Result
            .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .First();

        var matches = remoteFileMd5 == md5;

        Log.Information(
            "Remote file {Path} has md5 {RemoteMd5} which {DoesMatch} match given md5 {GivenMd5}", path,
            remoteFileMd5,
            matches ? "does" : "does not",
            md5
        );

        return matches;
    }


    string RenderDbInitScript(string script)
    {
        return script.Replace("$MYSQL_BACKEND_PASSWD", MySqlBackendPasswd);
    }

    #endregion
}
