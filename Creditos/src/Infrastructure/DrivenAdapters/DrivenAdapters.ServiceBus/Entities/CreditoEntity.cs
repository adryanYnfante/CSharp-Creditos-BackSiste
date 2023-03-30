namespace DrivenAdapters.ServiceBus.Entities;

public class CreditoEntity
{
    public string Id { get; set; }

    public decimal Monto { get; set; }

    public int PlazoEnMeses { get; set; }

    public int CuotasPagadas { get; set; }

    public decimal TasaDeInteres { get; set; }

    public decimal MontoPorCuota { get; set; }

    public decimal TotalIntereses { get; set; }

    public int CuotasRestantes { get; set; }

    public DateTime FechaDeSolicitud { get; set; }
}