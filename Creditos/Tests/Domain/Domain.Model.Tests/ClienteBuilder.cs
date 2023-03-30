using Domain.Model.Entities;

namespace Domain.Model.Tests;

public class ClienteBuilder
{
    private readonly Cliente cliente;

    public ClienteBuilder()
    {
        cliente = new Cliente();
    }

    public ClienteBuilder WithId(string id)
    {
        cliente.Id = id;
        return this;
    }

    public ClienteBuilder WithNombre(string nombre)
    {
        cliente.Nombre = nombre;
        return this;
    }

    public ClienteBuilder WithApellidos(string apellidos)
    {
        cliente.Apellidos = apellidos;
        return this;
    }

    public ClienteBuilder WithNumeroDeCedula(string numeroDeCedula)
    {
        cliente.NumeroDeCedula = numeroDeCedula;
        return this;
    }

    public ClienteBuilder WithCorreo(string correo)
    {
        cliente.Correo = correo;
        return this;
    }

    public ClienteBuilder WithNumeroDeTelefono(string numeroDeTelefono)
    {
        cliente.NumeroDeTelefono = numeroDeTelefono;
        return this;
    }

    public ClienteBuilder WithCredito(Credito credito)
    {
        cliente.AgregarCredito(credito);
        return this;
    }

    public Cliente Build()
    {
        return cliente;
    }
}