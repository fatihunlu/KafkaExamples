using MongoDB.Bson.Serialization.Attributes;

namespace KafkaDemo.Consumer.Models;

public class User
{
    [BsonId]
    public int Id { get; set; }

    // it's being ignored.
    [BsonElement("name")]
    public string Name { get; set; }

    [BsonElement("followers")]
    public List<Follower> Followers { get; set; } = new List<Follower>();
}

