using System.Threading.Tasks;

namespace Domain.Model.Entities.Gateway;

/// <summary>
/// Repositorio de credito
/// </summary>
public interface ICreditoRepository
{
    /// <summary>
    /// Método para persitir un crédito creado en la base de datos
    /// </summary>
    /// <param name="credito"></param>
    /// <returns></returns>
    Task<Credito> Crear(Credito credito);

    /// <summary>
    /// Método para actualizar los datos de un crédito en la base de datos
    /// </summary>
    /// <param name="credito"></param>
    /// <returns></returns>
    Task<Credito> ActualizarCredito(Credito credito);
}