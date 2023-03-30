using AutoMapper;
using credinet.exception.middleware.models;
using Domain.Model.Entities;
using Domain.Model.Entities.Gateway;
using DrivenAdapters.Mongo.Entities;
using Helpers.Commons.Exceptions;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace DrivenAdapters.Mongo;

/// <inheritdoc />
public class ClienteRepository : IClienteRepository
{
    private readonly IMapper _mapper;
    private readonly IMongoCollection<ClienteEntity> _coleccionClientes;
    private readonly FilterDefinitionBuilder<ClienteEntity> _filterDefinitionBuilder = Builders<ClienteEntity>.Filter;

    /// <summary>
    /// Consutrcutor de la clase para inyectar el mapper y el contexto de la base de datos
    /// </summary>
    /// <param name="mongoContext"></param>
    /// <param name="mapper"></param>
    public ClienteRepository(IContext mongoContext, IMapper mapper)
    {
        _mapper = mapper;
        _coleccionClientes = mongoContext.Clientes;
    }

    /// <inheritdoc />
    public async Task<Cliente> CrearAsync(Cliente cliente)
    {
        if (await ObtenerClientePorCedula(cliente.NumeroDeCedula) != null)
        {
            throw new BusinessException("El cliente que intenta registrar ya se encuentra en la base de datos",
                (int)TipoExcepcionNegocio.ElClienteYaEstaRegistrado);
        }

        var nuevoCliente = _mapper.Map<ClienteEntity>(cliente);

        await _coleccionClientes.InsertOneAsync(nuevoCliente);

        return _mapper.Map<Cliente>(nuevoCliente);
    }

    /// <inheritdoc />
    public async Task<Cliente> ObtenerAsync(string cedula)
    {
        var clienteEncontrado = await ObtenerClientePorCedula(cedula);

        if (clienteEncontrado == null)
        {
            throw new BusinessException("Éste cliente no se encuentra registrado en la base de datos", (int)TipoExcepcionNegocio.ExceptionAlIntentarObtenerDatosDeMongo);
        }

        return _mapper.Map<Cliente>(clienteEncontrado);
    }

    /// <inheritdoc />
    public async Task<Cliente> ActualizarAsync(Cliente cliente)
    {
        var result = await _coleccionClientes.ReplaceOneAsync(FiltroPorCedula(cliente.NumeroDeCedula), _mapper.Map<ClienteEntity>(cliente));

        if (result == null)
        {
            throw new BusinessException("Éste cliente no se encuentra registrado en la base de datos", (int)TipoExcepcionNegocio.ExceptionAlIntentarObtenerDatosDeMongo);
        }

        var clienteActualizado = await ObtenerClientePorCedula(cliente.NumeroDeCedula);

        return _mapper.Map<Cliente>(clienteActualizado);
    }

    private Task<ClienteEntity> ObtenerClientePorCedula(string cedula)
    {
        return FirstOrDefaultAsync(FiltroPorCedula(cedula));
    }

    private FilterDefinition<ClienteEntity> FiltroPorCedula(string cedula)
    {
        return _filterDefinitionBuilder.Eq(cliente => cliente.NumeroDeCedula, cedula);
    }

    private async Task<ClienteEntity> FirstOrDefaultAsync(FilterDefinition<ClienteEntity> filtro)
    {
        var cursor = await _coleccionClientes.FindAsync(filtro);

        return cursor.FirstOrDefault();
    }
}