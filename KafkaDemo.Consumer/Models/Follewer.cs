using MongoDB.Bson.Serialization.Attributes;

namespace KafkaDemo.Consumer.Models;

public class Follower
{
    [BsonElement("followerId")]
    public int FollowerId { get; set; }

    [BsonElement("followedAt")]
    public DateTime FollowedAt { get; set; }
}
