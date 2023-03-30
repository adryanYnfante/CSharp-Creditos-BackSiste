using DrivenAdapters.Mongo.Entities.Base;
using System.Collections.Generic;

namespace DrivenAdapters.Mongo.Entities;

/// <summary>
/// Entidad de cliente para guardar en base de datos
/// </summary>
public class ClienteEntity : EntityBase
{
    /// <summary>
    /// Nombre del cliente
    /// </summary>
    public string Nombre { get; set; }

    /// <summary>
    /// Apellidos del ciente
    /// </summary>
    public string Apellidos { get; set; }

    /// <summary>
    /// Numero de documento del cliente
    /// </summary>
    public string NumeroDeCedula { get; set; }

    /// <summary>
    /// Dirección de correo electrónico
    /// </summary>
    public string? Correo { get; set; }

    /// <summary>
    /// Número de telefono del cliente
    /// </summary>
    public string NumeroDeTelefono { get; set; }

    /// <summary>
    /// Lista de créditos del cliente
    /// </summary>
    public List<CreditoEntity> Creditos { get; set; }
}