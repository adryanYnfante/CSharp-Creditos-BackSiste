using DrivenAdapters.Mongo.Entities;
using MongoDB.Driver;
using System.Diagnostics.CodeAnalysis;

namespace DrivenAdapters.Mongo;

/// <summary>
/// Context is an implementation of <see cref="IContext"/>
/// </summary>
[ExcludeFromCodeCoverage]
public class Context : IContext
{
    private readonly IMongoDatabase _database;

    /// <summary>
    /// crea una nueva instancia de la clase <see cref="Context"/>
    /// </summary>
    /// <param name="connectionString"></param>
    /// <param name="databaseName"></param>
    public Context(string connectionString, string databaseName)
    {
        MongoClient _mongoClient = new MongoClient(connectionString);
        _database = _mongoClient.GetDatabase(databaseName);
    }

    /// <inheritdoc />
    public IMongoCollection<ClienteEntity> Clientes => _database.GetCollection<ClienteEntity>("Clientes");

    /// <inheritdoc />
    public IMongoCollection<CreditoEntity> Creditos => _database.GetCollection<CreditoEntity>("Creditos");
}