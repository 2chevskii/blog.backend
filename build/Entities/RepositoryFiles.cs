using System.Linq;
using Nuke.Common.IO;

namespace Dvchevskii.Blog.Build.Entities;

internal class RepositoryFiles(AbsolutePath rootDirectory)
{
    public AbsolutePath Root => rootDirectory;
    public AbsolutePath DockerComposeLocal => Root / "docker" / "docker-compose.local.yml";
    public AbsolutePath DockerCompose => Root / "docker" / "docker-compose.yml";
    public AbsolutePath MySqlInitDbDirectory => Root / "scripts" / "mysql" / "initdb.d";

    public AbsolutePath[] MySqlInitScripts => MySqlInitDbDirectory
        .GetFiles(pattern: "*.sql")
        .ToArray();
}
