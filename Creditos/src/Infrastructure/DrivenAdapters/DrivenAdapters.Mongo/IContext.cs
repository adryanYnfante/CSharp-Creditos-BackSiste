using DrivenAdapters.Mongo.Entities;
using MongoDB.Driver;

namespace DrivenAdapters.Mongo
{
    /// <summary>
    /// Interfaz Mongo context contract.
    /// </summary>
    public interface IContext
    {
        /// <summary>
        /// coleción para almacenar los clientes
        /// </summary>
        IMongoCollection<ClienteEntity> Clientes { get; }

        /// <summary>
        /// coleción para almacenar los creditos de un cliente
        /// </summary>
        IMongoCollection<CreditoEntity> Creditos { get; }
    }
}