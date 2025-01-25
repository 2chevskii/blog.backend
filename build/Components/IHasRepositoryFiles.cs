using Dvchevskii.Blog.Build.Entities;

namespace Dvchevskii.Blog.Build.Components;

interface IHasRepositoryFiles : INukeBuild
{
    RepositoryFiles RepositoryFiles => new RepositoryFiles(RootDirectory);
}
