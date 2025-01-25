using Nuke.Common.Tools.DotNet;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

namespace Dvchevskii.Blog.Build.Components;

interface IRestore : IHasSolution
{
    Target Restore => _ => _.Executes(() =>
    {
        DotNetRestore(s => s.SetProjectFile(Solution.Path));
    });
}
