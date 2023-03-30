using Domain.Model.Entities;

namespace Domain.Model.Tests;

public class CreditoBuilder
{
    private Credito credito;

    public CreditoBuilder()
    {
        credito = new Credito();
    }

    public CreditoBuilder WithId(string id)
    {
        credito.Id = id;
        return this;
    }

    public CreditoBuilder WithMonto(decimal monto)
    {
        credito.Monto = monto;
        return this;
    }

    public CreditoBuilder WithPlazoEnMeses(int plazoEnMeses)
    {
        credito.PlazoEnMeses = plazoEnMeses;
        return this;
    }

    public CreditoBuilder WithCantidadDeCuotas(int cantidadDeCuotas)
    {
        credito.CuotasPagadas = cantidadDeCuotas;
        return this;
    }

    public CreditoBuilder WithTasaDeInteres(decimal tasaDeInteres)
    {
        credito.TasaDeInteres = tasaDeInteres;
        return this;
    }

    public CreditoBuilder WithMontoPorCuota(decimal montoPorCuota)
    {
        credito.MontoPorCuota = montoPorCuota;
        return this;
    }

    public CreditoBuilder WithCuotasRestantes(int cuotasRestantes)
    {
        credito.CuotasRestantes = cuotasRestantes;
        return this;
    }

    public CreditoBuilder WithFechaDeSolicitud(DateTime fechaDeSolicitud)
    {
        credito.FechaDeSolicitud = fechaDeSolicitud;
        return this;
    }

    public CreditoBuilder WithPagos(List<Pago> pagos)
    {
        credito.Pagos = pagos;
        return this;
    }

    public Credito Build()
    {
        return credito;
    }
}