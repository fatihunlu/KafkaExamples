using System.Text.Json;
using Confluent.Kafka;
using KafkaDemo.Consumer.Events;
using MongoDB.Driver;

namespace KafkaDemo.Consumer;
public class KafkaService(IMongoClient mongoClient)
{
    private readonly UserService _userService = new(mongoClient);

    /// <summary>
    /// Consumes messages from the specified Kafka topic using the provided consumer group.
    /// </summary>
    /// <param name="topicName">The name of the Kafka topic to consume messages from.</param>
    /// <param name="groupId">The consumer group ID used to coordinate message consumption.</param>
    /// <param name="cancellationToken">A token to cancel the consumption process gracefully.</param>
    public async Task ConsumeMessages(string topicName, string groupId, CancellationToken cancellationToken)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = "localhost:9094",
            GroupId = groupId,
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = true
        };
        using var consumer = new ConsumerBuilder<string, string>(config).Build();

        try
        {
            consumer.Subscribe(topicName);
            Console.WriteLine($"Subscribed to topic 🗂 {topicName} with groupId: {groupId} \ud83d\udc65 ");
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = consumer.Consume(cancellationToken);
                    ProcessMessage(consumeResult.Message.Value);
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
    
    private void ProcessMessage(string message)
    {
        try
        {
            var followerEvent = JsonSerializer.Deserialize<FollowerEvent>(message);
            if (followerEvent == null) return;
            _userService.AddFollower(followerEvent.FolloweeId, followerEvent.FollowerId);
            
            Console.WriteLine($"📥 Consumed message: OrderCode: {message}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}