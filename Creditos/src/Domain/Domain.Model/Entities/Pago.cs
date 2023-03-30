using System;

namespace Domain.Model.Entities;

/// <summary>
/// Entidad que representa el pago de uan cuota de un crédito
/// </summary>
public class Pago
{
    /// <summary>
    /// Id
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Monto de la cuota
    /// </summary>
    public decimal Monto { get; set; }

    /// <summary>
    /// Fecha del pago
    /// </summary>
    public DateTime FechaDeCancelacion { get; set; } = DateTime.Now;
}