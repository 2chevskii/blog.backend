namespace Dvchevskii.Blog.Shared.Setup;

[AttributeUsage(AttributeTargets.Class)]
public sealed class SetupHandlerAttribute : Attribute
{
    public SetupBehaviour Behaviour { get; set; } = SetupBehaviour.OnStart;
    public string? SetupUser { get; set; }
    public int Order { get; set; } = int.MaxValue;
}
