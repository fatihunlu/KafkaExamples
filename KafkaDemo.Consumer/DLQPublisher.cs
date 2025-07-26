using Confluent.Kafka;
using KafkaDemo.Consumer.Events;
using System.Text.Json;

namespace KafkaDemo.Consumer;

public class DLQPublisher
{
    private const string DLQTopic = "user.follow-events.dlq";

    private readonly IProducer<string, string> _producer;

    public DLQPublisher(IProducer<string, string> producer)
    {
        _producer = producer;
    }

    public async Task SendToDLQAsync(string originalKey, string originalValue, string error)
    {
        var dlqEvent = new DLQFollowEvent
        {
            Key = originalKey,
            OriginalPayload = originalValue,
            ErrorMessage = error,
            FailedAt = DateTime.UtcNow
        };

        var json = JsonSerializer.Serialize(dlqEvent);

        var message = new Message<string, string>
        {
            Key = originalKey,
            Value = json
        };

        await _producer.ProduceAsync(DLQTopic, message);
        Console.WriteLine($"Sent to DLQ: {originalKey}");
    }
}
