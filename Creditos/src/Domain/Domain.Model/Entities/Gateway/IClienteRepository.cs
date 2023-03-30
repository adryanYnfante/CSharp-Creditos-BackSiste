using System.Threading.Tasks;

namespace Domain.Model.Entities.Gateway;

/// <summary>
/// Repositorio de clientes
/// </summary>
public interface IClienteRepository
{
    /// <summary>
    /// Método para persistir un cliente creado en la base de datos
    /// </summary>
    /// <param name="cliente"></param>
    /// <returns></returns>
    Task<Cliente> CrearAsync(Cliente cliente);

    /// <summary>
    /// Método para consultar un cliente por su cedula en la base de datos
    /// </summary>
    /// <param name="cedula"></param>
    /// <returns></returns>
    Task<Cliente> ObtenerAsync(string cedula);

    /// <summary>
    /// Método para actualizar un cliente en la base de datos
    /// </summary>
    /// <param name="cliente"></param>
    /// <returns></returns>
    Task<Cliente> ActualizarAsync(Cliente cliente);
}