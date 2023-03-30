using System.Collections.Generic;

namespace Domain.Model.Entities;

/// <summary>
/// Entidad de cliente.
/// </summary>
public class Cliente
{
    /// <summary>
    /// Constructor
    /// </summary>
    public Cliente()
    {
        Creditos = new List<Credito>();
    }

    /// <summary>
    /// Id
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Nombre del cliente
    /// </summary>
    public string Nombre { get; set; }

    /// <summary>
    /// Apellido del cliente
    /// </summary>
    public string Apellidos { get; set; }

    /// <summary>
    /// Numero de cedula del cliente
    /// </summary>
    public string NumeroDeCedula { get; set; }

    /// <summary>
    /// Correo electronico del cliente
    /// </summary>
    public string? Correo { get; set; }

    /// <summary>
    /// Numero telefónico del cliente
    /// </summary>
    public string NumeroDeTelefono { get; set; }

    /// <summary>
    /// Historial de creditos del cliente
    /// </summary>
    public List<Credito> Creditos { get; private set; }

    /// <summary>
    /// Método para agregar un credito al cliente
    /// </summary>
    /// <param name="credito"></param>
    public void AgregarCredito(Credito credito)
    {
        Creditos.Add(credito);
    }
}