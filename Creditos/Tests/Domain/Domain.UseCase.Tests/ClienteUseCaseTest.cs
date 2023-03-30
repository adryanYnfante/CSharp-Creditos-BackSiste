using Domain.Model.Entities;
using Domain.Model.Entities.Gateway;
using Domain.Model.Tests;
using Domain.UseCase.Clientes;
using Moq;
using Xunit;

namespace Domain.UseCase.Tests;

public class ClienteUseCaseTest
{
    private readonly MockRepository _mockRepository;

    private readonly Mock<IClienteRepository> repositoryMock;

    private readonly ClienteUseCase useCase;

    public ClienteUseCaseTest()
    {
        _mockRepository = new MockRepository(MockBehavior.Default);
        repositoryMock = _mockRepository.Create<IClienteRepository>();
        useCase = new(repositoryMock.Object);
    }

    [Fact]
    public async Task Crear()
    {
        var cliente = new ClienteBuilder()
            .WithNumeroDeCedula("123456789")
            .Build();

        repositoryMock.Setup((c) => c.CrearAsync(It.IsAny<Cliente>())).ReturnsAsync(cliente);

        var clienteCreado = await useCase.CrearCliente(cliente);

        Assert.Equal(cliente.NumeroDeCedula, clienteCreado.NumeroDeCedula);
    }

    [Fact]
    public async Task Actualizar()
    {
        var sinActualizar = new ClienteBuilder()
            .WithNombre("mario")
            .WithApellidos("bros")
            .WithNumeroDeCedula("123")
            .WithCorreo("mario@gmail.com")
            .WithNumeroDeTelefono("123")
            .Build();

        var actualizado = new ClienteBuilder()
            .WithNombre("wario")
            .WithApellidos("")
            .WithNumeroDeCedula("123")
            .WithCorreo("mario@gmail.com")
            .WithNumeroDeTelefono("2340923840")
            .Build();

        repositoryMock.Setup(repo => repo.ActualizarAsync(sinActualizar)).ReturnsAsync(actualizado);

        var result = await useCase.ActualizarDatosCliente(sinActualizar);

        Assert.NotEqual(sinActualizar, result);
        Assert.Equal(sinActualizar.NumeroDeCedula, result.NumeroDeCedula);
    }

    [Fact]
    public async Task Obtener()
    {
        var cliente = new ClienteBuilder()
            .WithNumeroDeCedula("123")
            .Build();

        repositoryMock.Setup(repo => repo.ObtenerAsync("123")).ReturnsAsync(cliente);

        var clienteEncontrado = await useCase.ObtenerCliente("123");

        Assert.NotNull(clienteEncontrado);
        Assert.Equal("123", clienteEncontrado.NumeroDeCedula);
    }
}