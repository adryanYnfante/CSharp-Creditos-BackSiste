using Xunit;

namespace Domain.Model.Tests;

public class ClienteTest
{
    [Fact]
    public void AgregarCredito()
    {
        var cliente = new ClienteBuilder().Build();
        var credito = new CreditoBuilder().Build();

        cliente.AgregarCredito(credito);

        Assert.NotEmpty(cliente.Creditos);
    }
}