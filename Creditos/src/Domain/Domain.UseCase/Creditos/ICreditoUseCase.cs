using Domain.Model.Entities;
using System.Threading.Tasks;

namespace Domain.UseCase.Creditos;

/// <summary>
/// Caso de uso de crédito para gestionar la creación, consulta y pago de cuotas de un crédito
/// </summary>
public interface ICreditoUseCase
{
    /// <summary>
    /// Método para la creación de un crédito
    /// </summary>
    /// <param name="credito"></param>
    /// <param name="numeroDeDocumento"></param>
    /// <returns></returns>
    Task<Credito> Crear(Credito credito, string numeroDeDocumento);

    /// <summary>
    ///Método para consultar los datos de un crédito de un cliente a través de su número de documento
    /// </summary>
    /// <param name="numeroDeDocumento"></param>
    /// <returns></returns>
    Task<Credito> Consultar(string numeroDeDocumento);

    /// <summary>
    /// Método para pagar la cuota de un crédito
    /// </summary>
    /// <param name="cuota"></param>
    /// <param name="numeroDeDocumento"></param>
    /// <returns></returns>
    Task<Credito> PagarCuota(Pago cuota, string numeroDeDocumento);
}