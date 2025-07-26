using Confluent.Kafka;
using System.Text.Json;

namespace KafkaDemo.Producer;

public class FollowEventProducer
{
    private const string TopicName = "user.follow-events";

    public async Task SendMessagesAsync()
    {
        var config = new ProducerConfig
        {
            BootstrapServers = "localhost:9094"
        };

        using var producer = new ProducerBuilder<string, string>(config).Build();

        int counter = 0;

        while (true)
        {
            var followerId = $"user-{counter}";
            var followeeId = (counter % 2 == 0) ? followerId : $"user-{counter + 1}"; // deliberately cause self-follow

            var payload = new
            {
                followerId,
                followeeId,
                timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            };

            var jsonValue = JsonSerializer.Serialize(payload);

            var message = new Message<string, string>
            {
                Key = Guid.NewGuid().ToString(),
                Value = jsonValue
            };

            var result = await producer.ProduceAsync(TopicName, message);
            Console.WriteLine($"Sent: {followerId} => {followeeId} | {result.TopicPartitionOffset}");

            counter++;
            await Task.Delay(2000); // wait 2 seconds before sending next message
        }
    }
}