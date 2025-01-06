using Dvchevskii.Blog.Shared.Contracts.Setup;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Dvchevskii.Blog.Shared.Setup;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSetupRunner(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddHostedService<SetupRunner>();
        return serviceCollection;
    }

    public static IServiceCollection AddSetupHandlers(
        this IServiceCollection serviceCollection,
        Predicate<Type>? filter = null
    )
    {
        var types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsAssignableTo(typeof(ISetupHandler)) && type is { IsClass: true, IsAbstract: false })
            .Where(type => filter?.Invoke(type) ?? true)
            .ToArray();

        var serviceDescriptors = types.Select(type =>
            ServiceDescriptor.Describe(typeof(ISetupHandler), type, ServiceLifetime.Scoped)
        ).ToList();

        serviceDescriptors.ForEach(serviceCollection.TryAddEnumerable);

        return serviceCollection;
    }
}
