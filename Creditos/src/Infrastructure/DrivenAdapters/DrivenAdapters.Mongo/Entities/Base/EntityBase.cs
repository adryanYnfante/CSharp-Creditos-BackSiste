using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DrivenAdapters.Mongo.Entities.Base;

/// <summary>
/// Entidad base para los documentos de mongo
/// </summary>
public class EntityBase
{
    /// <summary>
    /// Identificador del documento en mongo
    /// </summary>
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
}