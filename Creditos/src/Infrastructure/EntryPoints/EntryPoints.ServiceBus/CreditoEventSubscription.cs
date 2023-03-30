using Domain.UseCase.Common;
using EntryPoints.ServiceBus.Base;
using EntryPoints.ServiceBus.Dtos;
using Helpers.ObjectsUtils;
using Microsoft.Extensions.Options;
using org.reactivecommons.api;
using org.reactivecommons.api.domain;
using System.Reflection;

namespace EntryPoints.ServiceBus;

public class CreditoEventSubscription : SubscriptionBase, ICreditoEventSubscription
{
    private readonly IDirectAsyncGateway<CreditoRequest> _directAsyncGateway;
    private readonly IManageEventsUseCase _manageEventsUseCase;
    private readonly IOptions<ConfiguradorAppSettings> _appSettings;

    public CreditoEventSubscription(IDirectAsyncGateway<CreditoRequest> directAsyncGateway, IManageEventsUseCase manageEventsUseCase, IOptions<ConfiguradorAppSettings> appSettings) : base(manageEventsUseCase, appSettings)
    {
        _directAsyncGateway = directAsyncGateway;
        _manageEventsUseCase = manageEventsUseCase;
        _appSettings = appSettings;
    }

    public async Task SubscribeAsync()
    {
        await SubscribeOnEventAsync(_directAsyncGateway, _appSettings.Value.TopicName, _appSettings.Value.TopicNameSubscription, ProcesarCreditoCreado, MethodBase.GetCurrentMethod()!, maxConcurrentCalls: 1);
    }

    private async Task ProcesarCreditoCreado(DomainEvent<CreditoRequest> domainEvent) =>
           await HandleRequestAsync(async (credito) =>
               {
                   await _manageEventsUseCase.ConsoleLogAsync(domainEvent.name, domainEvent.eventId, domainEvent.data,
                       writeData: true);
               }, MethodBase.GetCurrentMethod()!, Guid.NewGuid().ToString(),
                          domainEvent);
}