using Domain.Model.Entities;
using System.Threading.Tasks;

namespace Domain.UseCase.Clientes;

/// <summary>
/// Caso de uso de cliente para la creación, consulta y actualización de datos de un cliente
/// </summary>
public interface IClienteUseCase
{
    /// <summary>
    /// Método para gestionar la creación de un cliente
    /// </summary>
    /// <param name="cliente"></param>
    /// <returns></returns>
    Task<Cliente> CrearCliente(Cliente cliente);

    /// <summary>
    /// Método para consultar los datos de un cliente a través de su cédula
    /// </summary>
    /// <param name="cedula"></param>
    /// <returns></returns>
    Task<Cliente> ObtenerCliente(string cedula);

    /// <summary>
    /// Método para actualizar los datos de un cliente
    /// </summary>
    /// <param name="cliente"></param>
    /// <returns></returns>
    Task<Cliente> ActualizarDatosCliente(Cliente cliente);
}