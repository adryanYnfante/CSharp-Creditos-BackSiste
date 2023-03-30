using Domain.Model.Entities;
using Domain.Model.Entities.Gateway;
using Domain.Model.Tests;
using Domain.UseCase.Creditos;
using Helpers.ObjectsUtils;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Domain.UseCase.Tests;

public class CreditoUseCaseTest
{
    private readonly MockRepository _mockRepository;

    private readonly Mock<ICreditoRepository> _creditoRepositoryMock;

    private readonly Mock<IClienteRepository> _clienteRepositoryMock;

    private readonly Mock<INotificationRepository> _notificationRepositoryMock;

    private readonly Mock<IOptions<ConfiguradorAppSettings>> _options;

    private readonly CreditoUseCase _useCase;

    public CreditoUseCaseTest()
    {
        _mockRepository = new MockRepository(MockBehavior.Default);
        _creditoRepositoryMock = _mockRepository.Create<ICreditoRepository>();
        _clienteRepositoryMock = _mockRepository.Create<IClienteRepository>();
        _notificationRepositoryMock = _mockRepository.Create<INotificationRepository>();
        _options = _mockRepository.Create<IOptions<ConfiguradorAppSettings>>();
        _options.Setup(o => o.Value).Returns(new ConfiguradorAppSettings() { TASA_EA = 0.4527M });
        _useCase = new(_notificationRepositoryMock.Object, _creditoRepositoryMock.Object, _clienteRepositoryMock.Object, _options.Object);
    }

    [Fact]
    public async Task CrearCredito()
    {
        var cedula = "123456789";
        var cliente = new ClienteBuilder()
            .WithNombre("david")
            .WithApellidos("sanchez")
            .WithCorreo("david@gmail.com")
            .WithNumeroDeCedula(cedula)
            .WithNumeroDeTelefono("3009873456")
            .Build();

        var credito = new CreditoBuilder()
            .WithMonto(10000000)
            .WithPlazoEnMeses(12)
            .Build();

        _clienteRepositoryMock.Setup(c => c.ObtenerAsync(cedula)).ReturnsAsync(cliente);
        _creditoRepositoryMock.Setup(c => c.Crear(It.IsNotNull<Credito>())).ReturnsAsync(credito);
        _clienteRepositoryMock.Setup(c => c.ActualizarAsync(It.IsAny<Cliente>())).ReturnsAsync(It.IsAny<Cliente>());

        //Act
        var creditoCreado = await _useCase.Crear(credito, cedula);

        Assert.True(creditoCreado.TasaDeInteres != 0);
        Assert.True(creditoCreado.MontoPorCuota != 0);
        Assert.True(creditoCreado.CuotasRestantes != 0);
        Assert.True(creditoCreado.CuotasRestantes == credito.PlazoEnMeses);
    }

    [Fact]
    public async Task ConsultarCredito()
    {
        var cedula = "123456789";
        var cliente = new ClienteBuilder()
            .WithNombre("david")
            .WithApellidos("sanchez")
            .WithCorreo("david@gmail.com")
            .WithNumeroDeCedula(cedula)
            .WithNumeroDeTelefono("3009873456")
            .Build();
        var credito1 = new CreditoBuilder().WithMonto(1000000M).WithPlazoEnMeses(12).Build();
        var credito2 = new CreditoBuilder().WithMonto(2000000M).WithPlazoEnMeses(24).Build();
        cliente.AgregarCredito(credito1);
        cliente.AgregarCredito(credito2);

        _clienteRepositoryMock.Setup(c => c.ObtenerAsync(It.IsAny<string>())).ReturnsAsync(cliente);

        var creditoEncontrado = await _useCase.Consultar(cedula);

        Assert.Equal(credito2.PlazoEnMeses, creditoEncontrado.PlazoEnMeses);
    }

    [Fact]
    public async Task PagarCuota()
    {
        var cedula = "123456789";
        var cuota = new PagoBuilder()
            .WithMonto(12000M)
            .Build();

        var cliente = new ClienteBuilder()
            .WithNombre("david")
            .WithApellidos("sanchez")
            .WithCorreo("david@gmail.com")
            .WithNumeroDeCedula(cedula)
            .WithNumeroDeTelefono("3009873456")
            .Build();

        var credito1 = new CreditoBuilder().WithMonto(1000000M).WithPlazoEnMeses(12).Build();
        var credito2 = new CreditoBuilder().WithMonto(2000000M).WithPlazoEnMeses(24).Build();
        cliente.AgregarCredito(credito1);
        cliente.AgregarCredito(credito2);

        _clienteRepositoryMock.Setup(c => c.ObtenerAsync(It.IsAny<string>())).ReturnsAsync(cliente);
        _clienteRepositoryMock.Setup(c => c.ActualizarAsync(It.IsAny<Cliente>())).ReturnsAsync(cliente);
        _creditoRepositoryMock.Setup(c => c.ActualizarCredito(It.IsAny<Credito>())).ReturnsAsync(credito2);
        _notificationRepositoryMock.Setup(n => n.Notificar(It.IsAny<Pago>()));
        _notificationRepositoryMock.Setup(n => n.NotificarEvento(It.IsAny<Pago>()));

        var creditoActualizado = await _useCase.PagarCuota(cuota, cedula);

        Assert.NotEmpty(creditoActualizado.Pagos);
    }
}