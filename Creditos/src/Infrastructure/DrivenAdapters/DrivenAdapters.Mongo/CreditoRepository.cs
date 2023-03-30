using AutoMapper;
using Domain.Model.Entities;
using Domain.Model.Entities.Gateway;
using DrivenAdapters.Mongo.Entities;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace DrivenAdapters.Mongo
{
    /// <inheritdoc />
    public class CreditoRepository : ICreditoRepository
    {
        private readonly IMapper _mapper;
        private readonly IClienteRepository _clienteRepository;
        private readonly IMongoCollection<CreditoEntity> _coleccionCreditos;
        private readonly FilterDefinitionBuilder<CreditoEntity> _filterDefinitionBuilder = Builders<CreditoEntity>.Filter;

        /// <summary>
        /// Constructor para inyectar las dependencias
        /// </summary>
        /// <param name="mongoContext"></param>
        /// <param name="mapper"></param>
        /// <param name="clienteRepository"></param>
        public CreditoRepository(IContext mongoContext, IMapper mapper, IClienteRepository clienteRepository)
        {
            _mapper = mapper;
            _clienteRepository = clienteRepository;
            _coleccionCreditos = mongoContext.Creditos;
        }

        /// <inheritdoc />
        public async Task<Credito> Crear(Credito credito)
        {
            var creditoEntity = _mapper.Map<CreditoEntity>(credito);

            await _coleccionCreditos.InsertOneAsync(creditoEntity);

            return _mapper.Map<Credito>(creditoEntity);
        }

        /// <inheritdoc />
        public async Task<Credito> ActualizarCredito(Credito credito)
        {
            var filter = _filterDefinitionBuilder.Eq(c => c.Id, credito.Id);

            await _coleccionCreditos.ReplaceOneAsync(filter, _mapper.Map<CreditoEntity>(credito));

            return credito;
        }
    }
}