using KafkaDemo.DLQHandler.Events;
using MongoDB.Driver;

namespace KafkaDemo.DLQHandler.Mongo;

public class DLQFollowEventRepository
{
    private readonly IMongoCollection<DLQFollowEvent> _collection;

    public DLQFollowEventRepository(IMongoClient client)
    {
        var database = client.GetDatabase("socialMedia_dlq");
        _collection = database.GetCollection<DLQFollowEvent>("follow_events_dlq");
    }

    public async Task InsertAsync(DLQFollowEvent evt)
    {
        await _collection.InsertOneAsync(evt);
        Console.WriteLine($"Inserted DLQ event to MongoDB: {evt.ErrorMessage}");
    }
}
