using AutoMapper;
using Domain.Model.Entities;
using Domain.UseCase.Clientes;
using EntryPoints.ReactiveWeb.Dtos;
using Grpc.Core;
using System;
using System.Threading.Tasks;

namespace EntryPoints.ReactiveWeb.GrpcServices;

public class ClienteGrpController : ClienteController.ClienteControllerBase
{
    private readonly IClienteUseCase _clienteUseCase;
    private readonly IMapper _mapper;

    public ClienteGrpController(IClienteUseCase clienteUseCase, IMapper mapper)
    {
        _clienteUseCase = clienteUseCase;
        _mapper = mapper;
    }

    public override async Task<ClienteReply> Crear(CrearClienteRequest request, ServerCallContext context)
    {
        Cliente clienteCreado = null;

        try
        {
            clienteCreado = await _clienteUseCase.CrearCliente(_mapper.Map<Cliente>(request));
        }
        catch (Exception e)
        {
            return new ClienteReply() { Error = true, Message = e.Message, Cliente = _mapper.Map<ClienteRequest>(request) };
        }

        return new ClienteReply() { Error = false, Message = "", Cliente = _mapper.Map<ClienteRequest>(clienteCreado) };
    }

    public override async Task<ClienteReply> Obtener(Cedula request, ServerCallContext context)
    {
        Cliente clienteEncontrado = null;

        try
        {
            clienteEncontrado = await _clienteUseCase.ObtenerCliente(request.NumeroDeCedula);
        }
        catch (Exception e)
        {
            return new ClienteReply()
            { Error = true, Message = e.Message, Cliente = new ClienteRequest() { NumeroDeCedula = request.NumeroDeCedula } };
        }

        return new ClienteReply()
        { Error = false, Message = "", Cliente = _mapper.Map<ClienteRequest>(clienteEncontrado) };
    }

    public override async Task<ClienteReply> Actualizar(ActualizarClienteRequest request, ServerCallContext context)
    {
        Cliente clienteActualizado = null;

        try
        {
            clienteActualizado = _mapper.Map<Cliente>(request);
            clienteActualizado = await _clienteUseCase.ActualizarDatosCliente(clienteActualizado);
        }
        catch (Exception e)
        {
            return new ClienteReply()
            { Error = true, Message = e.Message, Cliente = _mapper.Map<ClienteRequest>(clienteActualizado) };
        }

        return new ClienteReply()
        { Error = false, Message = "", Cliente = _mapper.Map<ClienteRequest>(clienteActualizado) };
    }
}