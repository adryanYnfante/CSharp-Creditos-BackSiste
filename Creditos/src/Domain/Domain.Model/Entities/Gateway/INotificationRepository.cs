using System.Threading.Tasks;

namespace Domain.Model.Entities.Gateway;

/// <summary>
/// Repositorio para notificar eventos
/// </summary>
public interface INotificationRepository
{
    /// <summary>
    /// Método para notificar un comando
    /// </summary>
    /// <param name="cuota"></param>
    /// <returns></returns>
    public Task Notificar(Pago cuota, string correo);

    /// <summary>
    /// Método para notificar un evento
    /// </summary>
    /// <param name="cuota"></param>
    /// <returns></returns>
    public Task NotificarEvento(Pago cuota);
}