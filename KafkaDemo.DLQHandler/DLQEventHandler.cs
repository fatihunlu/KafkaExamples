using Confluent.Kafka;
using KafkaDemo.DLQHandler.Events;
using KafkaDemo.DLQHandler.Mongo;
using MongoDB.Driver;
using System.Text.Json;

namespace KafkaDemo.DLQEventHandler;

public class DLQEventHandler(IMongoClient mongoClient)
{
    private readonly string _bootstrapServers = "localhost:9094";
    private readonly string _topic = "user.follow-events.dlq";
    private readonly DLQFollowEventRepository _repository = new(mongoClient);

    public async Task StartAsync()
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = _bootstrapServers,
            GroupId = "dlq-handler-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        consumer.Subscribe(_topic);

        Console.WriteLine("DLQ consumer started. Waiting for messages...");

        while (true)
        {
            try
            {
                var result = consumer.Consume();
                var message = result.Message.Value;

                var dlqEvent = JsonSerializer.Deserialize<DLQFollowEvent>(message);

                if (dlqEvent != null)
                {
                    await _repository.InsertAsync(dlqEvent);
                    Console.WriteLine($"Inserted DLQ event to MongoDB: {dlqEvent.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to process DLQ message: {ex.Message}");
            }
        }
    }
}