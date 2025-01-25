using Dvchevskii.Blog.Build.Entities;
using Nuke.Common.IO;

namespace Dvchevskii.Blog.Build.Components;

interface IHasDeploymentFiles : IHasDeploymentPath
{
    DeploymentFiles DeploymentFiles => new DeploymentFiles(DeploymentPath);
}
