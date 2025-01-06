using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Dvchevskii.Blog.Shared.Authentication;
using Dvchevskii.Blog.Shared.Authentication.Context;
using Dvchevskii.Blog.Shared.Authentication.Users;
using Dvchevskii.Blog.Shared.Contracts.Authentication.Context;
using Dvchevskii.Blog.Shared.Contracts.Authentication.Users;
using Dvchevskii.Blog.Shared.Contracts.Setup;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Dvchevskii.Blog.Shared.Setup;

public sealed class SetupRunner(
    ILogger<SetupRunner> logger,
    IAuthenticationContextProvider authenticationContextProvider,
    IServiceProvider serviceProvider
) : IHostedLifecycleService
{
    public Task StartAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    public Task StartedAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    public Task StartingAsync(CancellationToken cancellationToken) => RunOnStart(cancellationToken);

    public Task StoppedAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    public Task StoppingAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    public Task RunOnce(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    private async Task RunOnStart(CancellationToken cancellationToken)
    {
        logger.LogInformation("Running setup handlers with OnStart behaviour");

        using var serviceScope = serviceProvider.CreateScope();
        var setupHandlers = serviceScope.ServiceProvider.GetServices<ISetupHandler>();

        var handlerInfoArray = GetHandlerInfoArray(setupHandlers, SetupBehaviour.OnStart);

        if (handlerInfoArray.Length == 0)
        {
            logger.LogInformation("No setup handlers found");
            return;
        }

        logger.LogInformation("Total of {HandlerCount} handlers found", handlerInfoArray.Length);
        logger.LogDebug("Handler ordered list: {Handlers}", handlerInfoArray.Select(x => x.Handler.GetType().Name));

        foreach (var setupHandlerInfo in handlerInfoArray)
        {
            IAuthenticationScope? authenticationScope = null;
            if (setupHandlerInfo.HasUser)
            {
                var setupUser = setupHandlerInfo.GetKnownUserInfoForSetupUser();
                authenticationScope = authenticationContextProvider.CreateScope(
                    setupUser.ToAuthenticationData()
                );
            }

            try
            {
                await setupHandlerInfo.Handler.ExecuteAsync();
            }
            finally
            {
                authenticationScope?.Dispose();
            }
        }
    }

    private static SetupHandlerInfo? GetInfo(ISetupHandler handler)
    {
        var attribute = handler.GetType().GetCustomAttribute<SetupHandlerAttribute>();
        if (attribute == null)
        {
            return null;
        }

        return new SetupHandlerInfo(handler, attribute);
    }

    private SetupHandlerInfo[] GetHandlerInfoArray(
        IEnumerable<ISetupHandler> setupHandlers,
        SetupBehaviour setupBehaviour
    )
    {
        var handlerInfoArray = setupHandlers.Select(GetInfo)
            .Where(x => x.HasValue)
            .Select(x => x!.Value)
            .Where(x => x.Behaviour == setupBehaviour)
            .OrderBy(x => x.Order)
            .ToArray();

        return handlerInfoArray;
    }

    private readonly struct SetupHandlerInfo(ISetupHandler handler, SetupHandlerAttribute attribute)
    {
        public readonly ISetupHandler Handler = handler;
        public readonly SetupHandlerAttribute Attribute = attribute;

        public SetupBehaviour Behaviour => Attribute.Behaviour;

        [MemberNotNullWhen(true, nameof(SetupUser))]
        public bool HasUser => SetupUser != null;

        public string? SetupUser => Attribute.SetupUser;
        public int Order => Attribute.Order;

        public KnownUsers.KnownUserInfo GetKnownUserInfoForSetupUser()
        {
            if (!HasUser)
            {
                throw new InvalidOperationException("Handler does not have setup user specified");
            }

            var field = typeof(KnownUsers).GetField(SetupUser, BindingFlags.Public | BindingFlags.Static);

            if (field == null)
            {
                throw new InvalidOperationException($"Field {SetupUser} was not found in type {typeof(KnownUsers)}");
            }

            if (field.GetValue(null) is not KnownUsers.KnownUserInfo knownUserInfo)
            {
                throw new InvalidOperationException("Invalid field type");
            }

            return knownUserInfo;
        }
    }
}
