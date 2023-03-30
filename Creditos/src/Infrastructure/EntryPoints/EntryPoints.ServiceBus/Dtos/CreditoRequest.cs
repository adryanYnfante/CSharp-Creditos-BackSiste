namespace EntryPoints.ServiceBus.Dtos;

public class CreditoRequest
{
    public decimal Monto { get; set; }

    public int PlazoEnMeses { get; set; }

    public string numeroDeDocumento { get; set; }

    public string Correo { get; set; }
}