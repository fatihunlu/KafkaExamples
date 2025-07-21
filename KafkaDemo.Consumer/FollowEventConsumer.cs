using Confluent.Kafka;
using KafkaDemo.Consumer.Events;
using System.Text.Json;

namespace KafkaDemo.Consumer;

public class FollowEventConsumer
{
    private const string TopicName = "user.follow-events";

    public async Task ConsumeAsync()
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = "localhost:9094",
            GroupId = "follow-consumer-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        using var consumer = new ConsumerBuilder<string, string>(config).Build();
        using var dlqProducer = new ProducerBuilder<string, string>(new ProducerConfig
        {
            BootstrapServers = "localhost:9094"
        }).Build();

        var dlqPublisher = new DLQPublisher(dlqProducer);

        consumer.Subscribe(TopicName);

        Console.WriteLine("Listening to user.follow-events...");

        while (true)
        {
            try
            {
                var consumeResult = consumer.Consume();

                var json = consumeResult.Message.Value;
                var obj = JsonSerializer.Deserialize<FollowEvent>(json);

                if (obj == null)
                    throw new Exception("Invalid JSON payload");

                if (obj.FollowerId == obj.FolloweeId)
                    throw new Exception("User cannot follow themselves");

                Console.WriteLine($"Processed: {obj.FollowerId} → {obj.FolloweeId}");
            }
            catch (Exception ex)
            {
                await dlqPublisher.SendToDLQAsync(
                    originalKey: Guid.NewGuid().ToString(),
                    originalValue: ex.Message,
                    error: ex.ToString()
                );
            }
        }
    }
}