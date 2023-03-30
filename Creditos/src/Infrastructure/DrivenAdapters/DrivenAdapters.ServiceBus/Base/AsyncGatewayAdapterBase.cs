using Domain.UseCase.Common;
using Helpers.ObjectsUtils;
using Microsoft.Extensions.Options;
using org.reactivecommons.api;
using org.reactivecommons.api.domain;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace DrivenAdapter.ServicesBus.Base
{
    public class AsyncGatewayAdapterBase
    {
        private readonly IManageEventsUseCase _manageEventsUseCase;
        private readonly IOptions<ConfiguradorAppSettings> _appSettings;

        public AsyncGatewayAdapterBase(IManageEventsUseCase manageEventsUseCase,
            IOptions<ConfiguradorAppSettings> appSettings)
        {
            _manageEventsUseCase = manageEventsUseCase;
            _appSettings = appSettings;
        }

        /// <summary>
        /// Handle the send command.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="directAsyncGateway"></param>
        /// <param name="id"></param>
        /// <param name="data"></param>
        /// <param name="queue"></param>
        /// <param name="commandName"></param>
        /// <param name="methodBase"></param>
        /// <param name="callerMemberName"></param>
        /// <returns></returns>
        public async Task HandleSendCommandAsync<TData>(IDirectAsyncGateway<TData> directAsyncGateway, string id, TData data, string queue, string commandName,
            MethodBase methodBase, [CallerMemberName] string? callerMemberName = null)
        {
            Command<TData> command = new(commandName, id, data);

            string eventName = GetLogEventName(methodBase, callerMemberName);
            string message = $"[Id] : [{id}] - [Event] : [{eventName}]";

            try
            {
                _manageEventsUseCase.ConsoleDebugLog($"Inicia envío de comando: {message}. Data: {data}");

                await directAsyncGateway.SendCommand(queue, command);

                _manageEventsUseCase.ConsoleDebugLog($"comando enviado Exitosamente: {message}. Data: {data}");
            }
            catch (Exception ex)
            {
                _manageEventsUseCase.ConsoleErrorLog($"Error al enviar el comando: {message}. Data: {data}", ex);
            }
        }

        /// <summary>
        /// Handle the send event.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="directAsyncGateway"></param>
        /// <param name="id"></param>
        /// <param name="data"></param>
        /// <param name="topic"></param>
        /// <param name="eventName"></param>
        /// <param name="methodBase"></param>
        /// <param name="callerMemberName"></param>
        /// <returns></returns>
        public async Task HandleSendEventAsync<TData>(IDirectAsyncGateway<TData> directAsyncGateway, string id, TData data, string topic, string eventName,
            MethodBase methodBase, [CallerMemberName] string callerMemberName = null)
        {
            DomainEvent<TData> @event = new DomainEvent<TData>(eventName, id, data);

            string message = $"[Id] : [{id}] - [Event] : [{GetLogEventName(methodBase, callerMemberName)}]";

            try
            {
                _manageEventsUseCase.ConsoleDebugLog($"Inicia envío de evento: {message}. Data: {data}");

                await directAsyncGateway.SendEvent(topic, @event);

                _manageEventsUseCase.ConsoleDebugLog($"Evento enviado exitosamente: {message}. Data: {data}");
            }
            catch (Exception ex)
            {
                _manageEventsUseCase.ConsoleErrorLog($"Error al enviar el evento: {message}. Data: {data}", ex);
            }
        }

        #region Private methods

        /// <summary>
        /// Gets the log event name.
        /// </summary>
        /// <param name="methodBase"></param>
        /// <param name="callerMemberName"></param>
        /// <returns></returns>
        private string GetLogEventName(MethodBase methodBase, string? callerMemberName)
        {
            Type declaringType = methodBase.DeclaringType!.DeclaringType ?? methodBase.DeclaringType;

            return $"{_appSettings.Value.DomainName}.{declaringType?.Name}.{callerMemberName}";
        }

        #endregion Private methods
    }
}