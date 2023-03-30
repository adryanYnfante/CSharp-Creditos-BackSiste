using Domain.Model.Entities;
using Domain.Model.Entities.Gateway;
using System.Threading.Tasks;

namespace Domain.UseCase.Clientes;

/// <inheritdoc />
public class ClienteUseCase : IClienteUseCase
{
    private readonly IClienteRepository _clienteRepository;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="clienteRepository"></param>
    public ClienteUseCase(IClienteRepository clienteRepository)
    {
        _clienteRepository = clienteRepository;
    }

    /// <inheritdoc />
    public Task<Cliente> CrearCliente(Cliente cliente)
    {
        return _clienteRepository.CrearAsync(cliente);
    }

    /// <inheritdoc />
    public Task<Cliente> ObtenerCliente(string cedula)
    {
        return _clienteRepository.ObtenerAsync(cedula);
    }

    /// <inheritdoc />
    public Task<Cliente> ActualizarDatosCliente(Cliente cliente)
    {
        return _clienteRepository.ActualizarAsync(cliente);
    }
}