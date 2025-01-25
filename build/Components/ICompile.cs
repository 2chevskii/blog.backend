using Nuke.Common.Tools.DotNet;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

namespace Dvchevskii.Blog.Build.Components;

interface ICompile : IRestore
{
    Target Compile => _ => _.DependsOn<IRestore>()
        .Executes(() =>
        {
            DotNetBuild(s => s.SetProjectFile(Solution.Path));
        });
}
