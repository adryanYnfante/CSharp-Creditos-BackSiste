using AutoMapper;
using Domain.Model.Entities;
using Domain.UseCase.Clientes;
using Domain.UseCase.Common;
using EntryPoints.ReactiveWeb.Base;
using EntryPoints.ReactiveWeb.Dtos;
using Helpers.ObjectsUtils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace EntryPoints.ReactiveWeb.Controllers;

/// <summary>
/// Controllador de cliente, expone los servicios de creación, consulta y actualización de clientes
/// </summary>
[ApiController]
[Produces("application/json")]
[ApiVersion("1.0")]
[Route("api/[controller]")]
public class ClienteController : AppControllerBase<ClienteController>
{
    private readonly IMapper _mapper;
    private readonly IClienteUseCase _clienteUseCase;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="mapper"></param>
    /// <param name="clienteUseCase"></param>
    /// <param name="eventsService"></param>
    /// <param name="appSettings"></param>
    public ClienteController(IMapper mapper, IClienteUseCase clienteUseCase, IManageEventsUseCase eventsService, IOptions<ConfiguradorAppSettings> appSettings) : base(eventsService, appSettings)
    {
        _mapper = mapper;
        _clienteUseCase = clienteUseCase;
    }

    /// <summary>
    /// Método para crear un cliente, recibiendo sus datos en el body del request
    /// </summary>
    /// <param name="crearClienteDto"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> CrearCliente([FromBody] CrearClienteDto crearClienteDto) =>
        await HandleRequestAsync(async () => await _clienteUseCase.CrearCliente(_mapper.Map<Cliente>(crearClienteDto)), "");

    /// <summary>
    /// Método para consultar los datos de un cliente, recibiendo su número de cédula en la ruta del request
    /// </summary>
    /// <param name="cedula"></param>
    /// <returns></returns>
    [HttpGet("{cedula}")]
    public async Task<IActionResult> ConsultarCliente([FromRoute] string cedula) =>
          await HandleRequestAsync(async () => await _clienteUseCase.ObtenerCliente(cedula), "");

    /// <summary>
    /// Método para acutalizar datos del cliente recibiendo su número de cédula en la ruta del request y los datos a actualizar en el body del request
    /// </summary>
    /// <param name="cedula"></param>
    /// <param name="modificarClienteDto"></param>
    /// <returns></returns>
    [HttpPut("{cedula}")]
    public async Task<IActionResult> ActualizarDatosCliente([FromRoute] string cedula,
        [FromBody] ModificarClienteDto modificarClienteDto) =>
        await HandleRequestAsync(async () =>
        {
            var actualizacionCliente = _mapper.Map<Cliente>(modificarClienteDto);
            actualizacionCliente.NumeroDeCedula = cedula;

            return await _clienteUseCase.ActualizarDatosCliente(actualizacionCliente);
        }, "");
}