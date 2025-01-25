using Nuke.Common.IO;

namespace Dvchevskii.Blog.Build.Components;

interface IHasDeploymentPath : INukeBuild
{
    [Parameter] AbsolutePath DeploymentPath => TryGetValue(() => DeploymentPath);
}
