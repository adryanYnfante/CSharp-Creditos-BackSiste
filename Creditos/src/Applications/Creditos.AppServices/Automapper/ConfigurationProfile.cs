using AutoMapper;
using Domain.Model.Entities;
using DrivenAdapters.Mongo.Entities;
using EntryPoints.ReactiveWeb.Dtos;

namespace Creditos.AppServices.Automapper
{
    /// <summary>
    /// EntityProfile
    /// </summary>
    public class ConfigurationProfile : Profile
    {
        /// <summary>
        /// ConfigurationProfile
        /// </summary>
        public ConfigurationProfile()
        {
            CreateMap<CrearClienteDto, Cliente>().ReverseMap();
            CreateMap<CreditoEntity, Credito>().ReverseMap();
            CreateMap<PagoEntity, Pago>().ReverseMap();
            CreateMap<CrearCreditoRequest, Credito>().ReverseMap();
            CreateMap<Pago, DrivenAdapters.ServiceBus.Entities.PagoEntity>().ReverseMap();
            CreateMap<PagoDto, Pago>().ReverseMap();
            CreateMap<Cliente, ClienteEntity>().ReverseMap();
            CreateMap<Cliente, ClienteRequest>().ReverseMap();
            CreateMap<ModificarClienteDto, Cliente>().ReverseMap();
            CreateMap<CrearClienteRequest, Cliente>().ReverseMap();
            CreateMap<CrearClienteRequest, ClienteRequest>().ReverseMap();
            CreateMap<ActualizarClienteRequest, Cliente>()
                .ForMember((cliente) => cliente.NumeroDeCedula,
                    (opt) => opt.MapFrom((request) => request.Cedula.NumeroDeCedula));
        }
    }
}