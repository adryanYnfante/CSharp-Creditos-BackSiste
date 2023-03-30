namespace EntryPoints.ReactiveWeb.Dtos;

public class CrearClienteDto
{
    public string Nombre { get; set; }

    public string Apellidos { get; set; }

    public string NumeroDeCedula { get; set; }

    public string? Correo { get; set; }

    public string NumeroDeTelefono { get; set; }
}