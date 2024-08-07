using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MyWebApi.Domain.Models;

public class Entity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    private DateTime? _createdDate;
    private DateTime? _modifiedDate;

    public DateTime? CreateDate
    {
        get => _createdDate ?? DateTime.Now;
        set => _createdDate = value;
    }

    public DateTime ModifiedDate
    {
        get => _modifiedDate ?? DateTime.Now;
        set => _modifiedDate = value;
    }
}