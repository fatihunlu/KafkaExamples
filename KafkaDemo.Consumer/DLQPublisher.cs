using Confluent.Kafka;

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
        var message = new Message<string, string>
        {
            Key = originalKey,
            Value = $"ERROR: {error} | PAYLOAD: {originalValue}"
        };

        await _producer.ProduceAsync(DLQTopic, message);
        Console.WriteLine($"Sent to DLQ: {originalKey} | Reason: {error}");
    }
}
