using credinet.exception.middleware.models;
using Domain.Model.Entities;
using Domain.Model.Entities.Gateway;
using Helpers.Commons.Exceptions;
using Helpers.ObjectsUtils;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.UseCase.Creditos
{
    /// <inheritdoc />
    public class CreditoUseCase : ICreditoUseCase
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly ICreditoRepository _creditoRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly ConfiguradorAppSettings _options;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="notificationRepository"></param>
        /// <param name="creditoRepository"></param>
        /// <param name="clienteRepository"></param>
        /// <param name="options"></param>
        public CreditoUseCase(INotificationRepository notificationRepository, ICreditoRepository creditoRepository, IClienteRepository clienteRepository, IOptions<ConfiguradorAppSettings> options)
        {
            _notificationRepository = notificationRepository;
            _creditoRepository = creditoRepository;
            _clienteRepository = clienteRepository;
            _options = options.Value;
        }

        /// <inheritdoc />
        public async Task<Credito> Crear(Credito credito, string numeroDeDocumento)
        {
            var cliente = await _clienteRepository.ObtenerAsync(numeroDeDocumento);

            if (cliente.Creditos.Count > 0 && cliente.Creditos[cliente.Creditos.Count - 1].CuotasRestantes != 0)
                throw new BusinessException("El cliente ya tiene un crédito activo",
                    (int)TipoExcepcionNegocio.ElClienteYaTieneUnCreditoActivo);

            credito.CalcularTasaInteres(_options.TASA_EA);
            credito.CalcularCuota();
            credito.CalcularTotalIntereses();
            credito.CuotasRestantes = credito.PlazoEnMeses;

            var creditoGuardado = await _creditoRepository.Crear(credito);
            cliente.Creditos.Add(creditoGuardado);
            await _clienteRepository.ActualizarAsync(cliente);

            return creditoGuardado;
        }

        /// <inheritdoc />
        public async Task<Credito> Consultar(string numeroDeDocumento)
        {
            var cliente = await _clienteRepository.ObtenerAsync(numeroDeDocumento);

            return cliente.Creditos.Last(c => true);
        }

        /// <inheritdoc />
        public async Task<Credito> PagarCuota(Pago cuota, string numeroDeDocumento)
        {
            var credito = await Consultar(numeroDeDocumento);
            var cliente = await _clienteRepository.ObtenerAsync(numeroDeDocumento);

            credito.PagarCuota(cuota);
            cliente.Creditos.FirstOrDefault(c => c.Id == credito.Id)!.Pagos.Add(cuota);

            await _creditoRepository.ActualizarCredito(credito);
            await _clienteRepository.ActualizarAsync(cliente);

            await _notificationRepository.Notificar(cuota, cliente.Correo);
            await _notificationRepository.NotificarEvento(cuota);

            return credito;
        }
    }
}