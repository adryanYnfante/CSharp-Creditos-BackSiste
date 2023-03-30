using AutoMapper;
using Domain.Model.Entities;
using Domain.Model.Entities.Gateway;
using Domain.UseCase.Common;
using DrivenAdapter.ServicesBus.Base;
using DrivenAdapters.ServiceBus.Entities;
using Helpers.ObjectsUtils;
using Microsoft.Extensions.Options;
using org.reactivecommons.api;
using System.Reflection;

namespace DrivenAdapters.ServiceBus;

public class NotificationRepository : AsyncGatewayAdapterBase, INotificationRepository
{
    private readonly IDirectAsyncGateway<PagoEntity> _directAsyncGateway;
    private readonly IMapper _mapper;
    private readonly IOptions<ConfiguradorAppSettings> _appSettings;

    public NotificationRepository(IDirectAsyncGateway<PagoEntity> directAsyncGateway, IMapper mapper, IManageEventsUseCase manageEventsUseCase, IOptions<ConfiguradorAppSettings> appSettings) : base(manageEventsUseCase, appSettings)
    {
        _directAsyncGateway = directAsyncGateway;
        _mapper = mapper;
        _appSettings = appSettings;
    }

    public async Task Notificar(Pago cuota, string correo)
    {
        var pago = _mapper.Map<PagoEntity>(cuota);
        pago.Correo = correo;
        await HandleSendCommandAsync(_directAsyncGateway, cuota.Id, pago, _appSettings.Value.QueueName, "Pago", MethodBase.GetCurrentMethod());
    }

    public async Task NotificarEvento(Pago cuota)
    {
        await HandleSendEventAsync(_directAsyncGateway, cuota.Id, _mapper.Map<PagoEntity>(cuota),
             _appSettings.Value.TopicName, "PagoExitoso", MethodBase.GetCurrentMethod());
    }
}