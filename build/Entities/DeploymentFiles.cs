using System.IO;
using Nuke.Common.IO;

namespace Dvchevskii.Blog.Build.Entities;

internal class DeploymentFiles(AbsolutePath deploymentDirectory)
{
    public AbsolutePath Root => deploymentDirectory;
    public AbsolutePath DockerCompose => Root / "infrastructure" / "docker-compose.yml";
    public AbsolutePath MySqlInitDbDirectory => Root / "infrastructure" / "mysql" / "initdb.d";
    public AbsolutePath MySqlDataDirectory => Root / "infrastructure" / "mysql" / "data";

    public AbsolutePath[] RequiredDirectories =>
    [
        Root,
        Root / "infrastructure",
        Root / "infrastructure" / "mysql",
        Root / "infrastructure" / "mysql" / "data",
        Root / "infrastructure" / "mysql" / "initdb.d",
    ];

    public AbsolutePath GetMySqlInitScriptPath(FileInfo fileInfo)
    {
        return MySqlInitDbDirectory / fileInfo.Name;
    }
}
