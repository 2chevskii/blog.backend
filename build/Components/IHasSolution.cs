using Nuke.Common.ProjectModel;

namespace Dvchevskii.Blog.Build.Components;

interface IHasSolution : INukeBuild
{
    [Solution] Solution Solution => TryGetValue(() => Solution);
}
