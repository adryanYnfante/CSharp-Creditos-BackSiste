using AutoMapper;
using Domain.Model.Entities;
using Domain.UseCase.Common;
using Domain.UseCase.Creditos;
using EntryPoints.ReactiveWeb.Base;
using EntryPoints.ReactiveWeb.Dtos;
using Helpers.ObjectsUtils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace EntryPoints.ReactiveWeb.Controllers;

/// <summary>
/// Controllador que expone los servicios de creación, consulta y actualización de créditos
/// </summary>
[ApiController]
[Produces("application/json")]
[ApiVersion("1.0")]
[Route("api/[controller]")]
public class CreditoController : AppControllerBase<CreditoController>
{
    private readonly IMapper _mapper;
    private readonly ICreditoUseCase _useCase;
    private readonly IOptions<ConfiguradorAppSettings> _appSettings;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="mapper"></param>
    /// <param name="useCase"></param>
    /// <param name="eventsService"></param>
    /// <param name="appSettings"></param>
    public CreditoController(IMapper mapper, ICreditoUseCase useCase, IManageEventsUseCase eventsService, IOptions<ConfiguradorAppSettings> appSettings) : base(eventsService, appSettings)
    {
        _mapper = mapper;
        _useCase = useCase;
        _appSettings = appSettings;
    }

    /// <summary>
    /// Método para crear un crédito, recibe un objeto de tipo CrearCreditoRequest en el body
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("solicitar")]
    public async Task<IActionResult> CrearCredito(CrearCreditoRequest request) => await
        HandleRequestAsync(async () => await _useCase.Crear(_mapper.Map<Credito>(request), request.numeroDeDocumento), "");

    /// <summary>
    /// Método para consultar un crédito por número de cédula del cliente
    /// </summary>
    /// <param name="cedula"></param>
    /// <returns></returns>
    [HttpGet("{cedula}")]
    public async Task<IActionResult> ConsultarCredito([FromRoute] string cedula) => await
        HandleRequestAsync(async () => await _useCase.Consultar(cedula), "");

    /// <summary>
    /// Método para pagar una cuota de un crédito, recibe un objeto de tipo PagoDto en el body
    /// </summary>
    /// <param name="cedula"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("pagar/{cedula}")]
    public async Task<IActionResult> PagarCuota([FromRoute] string cedula, [FromBody] PagoDto request) => await
        HandleRequestAsync(async () => await _useCase.PagarCuota(_mapper.Map<Pago>(request), cedula), "");
}