using Confluent.Kafka;

namespace KafkaDemo.Consumer;

public class KafkaService
{
    public async Task ConsumeMessages(string topicName, CancellationToken cancellationToken)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = "localhost:9094",
            GroupId = "consumer-group-one",
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = true
        };

        using var consumer = new ConsumerBuilder<string, string>(config).Build();

        try
        {
            consumer.Subscribe(topicName);
            
            Console.WriteLine($"Subscribed to topic 🗂 {topicName}");
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = consumer.Consume(cancellationToken);
                    Console.WriteLine($"📥 Consumed message: {consumeResult.Message.Key} - {consumeResult.Message.Value} " +
                                      $"from topic {consumeResult.Topic}, partition {consumeResult.Partition}, offset {consumeResult.Offset}");
                }
                catch (ConsumeException ex)
                {
                    Console.WriteLine($"❌ Error consuming message: {ex.Error.Reason}");
                }
            }
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Consumption was canceled.");
        }
        finally
        {
            consumer.Close();
            Console.WriteLine("Consumer closed.");
        }
    }
}