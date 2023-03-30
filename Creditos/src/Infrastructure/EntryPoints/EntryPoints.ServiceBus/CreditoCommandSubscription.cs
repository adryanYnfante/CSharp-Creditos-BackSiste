using Domain.UseCase.Common;
using EntryPoints.ServiceBus.Base;
using EntryPoints.ServiceBus.Dtos;
using Helpers.ObjectsUtils;
using Microsoft.Extensions.Options;
using org.reactivecommons.api;
using org.reactivecommons.api.domain;
using System.Reflection;
using MimeKit;

namespace EntryPoints.ServiceBus;

public class CreditoCommandSubscription : SubscriptionBase, ICreditoCommandSubscription
{
    private readonly IOptions<MailSettings> _mailOptions;
    private readonly IDirectAsyncGateway<CreditoRequest> _directAsyncGateway;
    private readonly IManageEventsUseCase _manageEventsUseCase;
    private readonly ConfiguradorAppSettings _appSettingsValue;

    public CreditoCommandSubscription(IOptions<MailSettings> mailOptions, IDirectAsyncGateway<CreditoRequest> directAsyncGateway, IManageEventsUseCase manageEventsUseCase, IOptions<ConfiguradorAppSettings> appSettings) : base(manageEventsUseCase, appSettings)
    {
        _mailOptions = mailOptions;
        _directAsyncGateway = directAsyncGateway;
        _manageEventsUseCase = manageEventsUseCase;
        _appSettingsValue = appSettings.Value;
    }

    public async Task SubscribeAsync()
    {
        await SubscribeOnCommandAsync(_directAsyncGateway, _appSettingsValue.QueueName, Logger, MethodBase.GetCurrentMethod()!, maxConcurrentCalls: 1);
    }

    private async Task Logger(Command<CreditoRequest> command) =>
        await HandleRequestAsync(async (credito) =>
            {
                var mensaje = new MimeMessage();
                mensaje.From.Add(new MailboxAddress(_mailOptions.Value.SenderName, _mailOptions.Value.SenderEmail));
                mensaje.To.Add(new MailboxAddress("", credito.Correo));
                mensaje.Subject = "Pago exitoso!";
                mensaje.Body = new TextPart("html") { Text = "<h1>Pago exitoso 🐀</h1>" };

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    await client.ConnectAsync(_mailOptions.Value.Server);
                    await client.AuthenticateAsync(_mailOptions.Value.UserName, _mailOptions.Value.Password);
                    await client.SendAsync(mensaje);
                    await client.DisconnectAsync(true);
                };
            }, MethodBase.GetCurrentMethod()!, Guid.NewGuid().ToString(),
            command);
}