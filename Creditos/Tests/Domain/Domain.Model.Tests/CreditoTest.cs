using Xunit;

namespace Domain.Model.Tests;

public class CreditoTest
{
    [Fact]
    public void PagarCuota()
    {
        var credito = new CreditoBuilder()
            .WithMonto(10000000)
            .WithPlazoEnMeses(12)
            .WithMontoPorCuota(1014287.69M)
            .WithCuotasRestantes(12)
            .Build();

        var pago = new PagoBuilder()
            .WithMonto(1014287.69M)
            .Build();

        credito.PagarCuota(pago);

        Assert.Equal(1, credito.CuotasPagadas);
        Assert.Equal(11, credito.CuotasRestantes);
    }

    [Fact]
    public void CalcularTasaInteres()
    {
        var credito = new CreditoBuilder()
            .WithMonto(10000000)
            .WithPlazoEnMeses(12)
            .Build();

        decimal tasaInteresEA = 0.4527M;

        credito.CalcularTasaInteres(tasaInteresEA);

        Assert.Equal(0.0316079049744291M, credito.TasaDeInteres);
    }

    [Fact]
    public void CalcularCuota()
    {
        var credito = new CreditoBuilder()
            .WithMonto(10000000)
            .WithPlazoEnMeses(12)
            .WithTasaDeInteres(0.0316079049744291M)
            .Build();

        credito.CalcularCuota();

        Assert.Equal(1014287.69M, credito.MontoPorCuota);
    }

    [Fact]
    public void CalcularTotalIntereses()
    {
        var credito = new CreditoBuilder()
            .WithMonto(10000000)
            .WithPlazoEnMeses(12)
            .WithTasaDeInteres(0.0316079049744291M)
            .WithMontoPorCuota(1014287.69M)
            .Build();

        credito.CalcularTotalIntereses();

        Assert.Equal(2171452.28M, credito.TotalIntereses);
    }
}