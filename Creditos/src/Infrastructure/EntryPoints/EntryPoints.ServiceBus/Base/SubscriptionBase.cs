using Domain.UseCase.Common;
using Helpers.ObjectsUtils;
using Microsoft.Extensions.Options;
using org.reactivecommons.api;
using org.reactivecommons.api.domain;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace EntryPoints.ServiceBus.Base;

public class SubscriptionBase
{
    private readonly IManageEventsUseCase _manageEventsUseCase;
    public readonly IOptions<ConfiguradorAppSettings> _appSettings;

    public SubscriptionBase(
       IManageEventsUseCase manageEventsUseCase,
       IOptions<ConfiguradorAppSettings> appSettings)
    {
        _manageEventsUseCase = manageEventsUseCase;
        _appSettings = appSettings;
    }

    /// <summary>
    /// Subscribe on command
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="directAsyncGateway"></param>
    /// <param name="targetName"></param>
    /// <param name="subscriptionName"></param>
    /// <param name="handler"></param>
    /// <param name="methodBase"></param>
    /// <param name="maxConcurrentCalls"></param>
    /// <param name="callerMemberName"></param>
    /// <returns></returns>
    public async Task SubscribeOnCommandAsync<T>(IDirectAsyncGateway<T> directAsyncGateway,
            string targetName, Func<Command<T>, Task> handler, MethodBase methodBase, int maxConcurrentCalls = 1, [CallerMemberName] string callerMemberName = null)
    {
        try
        {
            string eventName = $"{_appSettings.Value.DomainName}.{methodBase.DeclaringType.DeclaringType.Name}.{callerMemberName}";
            await _manageEventsUseCase.ConsoleLogAsync(eventName, callerMemberName, data: null);
            await directAsyncGateway.SuscripcionCommand(targetName, handler, maxConcurrentCalls);
        }
        catch (Exception ex)
        {
            _manageEventsUseCase.ConsoleErrorLog(ex.Message, ex);
            await LogAsync(ex, methodBase, targetName, new { targetName }, callerMemberName);
        }
    }

    /// <summary>
    /// Subscribe on event
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="directAsyncGateway"></param>
    /// <param name="targetName"></param>
    /// <param name="subscriptionName"></param>
    /// <param name="handler"></param>
    /// <param name="methodBase"></param>
    /// <param name="maxConcurrentCalls"></param>
    /// <param name="callerMemberName"></param>
    /// <returns></returns>
    public async Task SubscribeOnEventAsync<T>(IDirectAsyncGateway<T> directAsyncGateway,
        string targetName, string subscriptionName, Func<DomainEvent<T>, Task> handler, MethodBase methodBase, int maxConcurrentCalls = 1,
        [CallerMemberName] string callerMemberName = null)
    {
        try
        {
            string eventName = $"{_appSettings.Value.DomainName}.{methodBase.DeclaringType.DeclaringType.Name}.{callerMemberName}";

            await _manageEventsUseCase.ConsoleLogAsync(eventName, callerMemberName, data: null);

            await directAsyncGateway.SuscripcionEvent(targetName, subscriptionName, handler);
        }
        catch (Exception ex)
        {
            _manageEventsUseCase.ConsoleErrorLog(ex.Message, ex);
            await LogAsync(ex, methodBase, targetName, new { targetName, subscriptionName }, callerMemberName);
        }
    }

    /// <summary>
    /// Handle request domain event
    /// </summary>
    /// <param name="serviceHandler"></param>
    /// <param name="methodBase"></param>
    /// <param name="logId"></param>
    /// <param name="request"></param>
    /// <param name="logBusinessException"></param>
    /// <param name="callerMemberName"></param>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    public async Task HandleRequestAsync<TEntity>(Func<TEntity, Task> serviceHandler, MethodBase methodBase, string logId, DomainEvent<TEntity> request,
            [CallerMemberName] string? callerMemberName = null) =>
            await HandleAsync(async () =>
            {
                _manageEventsUseCase.ConsoleInfoLog("Se inicia procesamiento del evento {@callerMemberName} {@request}", callerMemberName, request);
                Validate(request);
                return await InvokeAsync(async (entity) =>
                {
                    await serviceHandler(entity);
                    _manageEventsUseCase.ConsoleInfoLog("Se finaliza procesamiento del evento {@callerMemberName} {@request}", callerMemberName, request);
                    return true;
                },
                    request.data);
            },
            methodBase,
            logId,
            request,
            callerMemberName);

    /// <summary>
    /// Handle request command
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="serviceHandler"></param>
    /// <param name="methodBase"></param>
    /// <param name="logId"></param>
    /// <param name="request"></param>
    /// <param name="logBusinessException"></param>
    /// <param name="callerMemberName"></param>
    /// <returns></returns>
    public async Task HandleRequestAsync<TEntity>(Func<TEntity, Task> serviceHandler, MethodBase methodBase, string logId, Command<TEntity> request,
        [CallerMemberName] string callerMemberName = null) =>
        await HandleAsync(async () =>
        {
            _manageEventsUseCase.ConsoleInfoLog("Se inicia procesamiento del commando {@callerMemberName} {@request}", callerMemberName, request);
            Validate(request);
            return await InvokeAsync(async (entity) =>
            {
                await serviceHandler(entity);
                _manageEventsUseCase.ConsoleInfoLog("Se finaliza procesamiento del comando {@callerMemberName} {@request}", callerMemberName, request);
                return true;
            },
                request.data);
        },
            methodBase,
            logId,
            request,
            callerMemberName);

    #region Private

    /// <summary>
    /// Handle request
    /// </summary>
    /// <param name="serviceHandler"></param>
    /// <param name="methodBase"></param>
    /// <param name="logId"></param>
    /// <param name="request"></param>
    /// <param name="callerMemberName"></param>
    /// <typeparam name="TResult"></typeparam>
    /// <returns></returns>
    private async Task<TResult> HandleAsync<TResult, TRequest>(Func<Task<TResult>> serviceHandler, MethodBase methodBase, string logId, TRequest request,
        [CallerMemberName] string callerMemberName = null)
    {
        try
        {
            string eventName = $"{_appSettings.Value.DomainName}.{methodBase.DeclaringType.DeclaringType.Name}.{callerMemberName}";
            await _manageEventsUseCase.ConsoleLogAsync(eventName, logId, data: null);
            return await serviceHandler();
        }
        catch (Exception ex)
        {
            _manageEventsUseCase.ConsoleErrorLog(ex.Message, ex);
            await LogAsync(ex, methodBase, logId, request, callerMemberName);
            throw;
        }
    }

    /// <summary>
    /// Invoke
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="handler"></param>
    /// <param name="entity"></param>
    /// <returns></returns>
    private async Task<TResult> InvokeAsync<TEntity, TResult>(Func<TEntity, Task<TResult>> handler, TEntity entity)
    {
        return await handler(entity);
    }

    /// <summary>
    /// Validate
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="command"></param>
    private void Validate<T>(Command<T> command)
    {
        if (command == null || command.data == null)
        {
            throw new ArgumentNullException(nameof(command));
        }
    }

    /// <summary>
    /// Validate
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="event"></param>
    private void Validate<T>(DomainEvent<T> @event)
    {
        if (@event == null || @event.data == null)
        {
            throw new ArgumentNullException(nameof(@event));
        }
    }

    /// <summary>
    /// Log
    /// </summary>
    /// <param name="ex"></param>
    /// <param name="methodBase"></param>
    /// <param name="logId"></param>
    /// <param name="request"></param>
    /// <param name="callerMemberName"></param>
    /// <returns></returns>
    private async Task LogAsync(Exception ex, MethodBase methodBase, string logId, dynamic request, [CallerMemberName] string callerMemberName = null)
    {
        object logDetails = GetLogDetails(ex, request);
        string eventName = $"Exception.{_appSettings.Value.DomainName}.{methodBase.DeclaringType.DeclaringType.Name}.{callerMemberName}.{ex.GetType().Name}";
        await _manageEventsUseCase.ConsoleLogAsync(eventName, logId, logDetails, writeData: true);
    }

    /// <summary>
    /// Get log details
    /// </summary>
    /// <param name="ex"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    private object GetLogDetails(Exception ex, dynamic request) =>
            new
            {
                exception = (ex != null) ? ex.ToString() : string.Empty,
                request
            };

    #endregion Private
}