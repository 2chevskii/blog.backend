namespace Dvchevskii.Blog.Application.Services.Setup;

[AttributeUsage(AttributeTargets.Class)]
public sealed class SetupHandlerAttribute : Attribute
{
    public SetupBehaviour Behaviour { get; set; } = SetupBehaviour.OnStart;
    public string? SetupUser { get; set; }
    public int Order { get; set; } = int.MaxValue;
}
