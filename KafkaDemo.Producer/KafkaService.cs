using System.Text.Json;
using Confluent.Kafka;
using Confluent.Kafka.Admin;
using KafkaDemo.Producer.Events;

namespace KafkaDemo.Producer;

public class KafkaService
{
    /// <summary>
    /// Creates a Kafka topic with the specified name, number of partitions, and replication factor.
    /// </summary>
    /// <param name="topicName">The name of the topic to be created.</param>
    /// <param name="numPartitions">The number of partitions for the topic (default is 2).</param>
    /// <param name="replicationFactor">The replication factor for the topic (default is 1).</param>
    public async Task CreateTopicAsync(string topicName, int numPartitions = 2, short replicationFactor = 1)
    {
        using var client = new AdminClientBuilder(new AdminClientConfig
        {
            BootstrapServers = "localhost:9094",
        }).Build();

        try
        {
            var topicSpecification = new TopicSpecification
            {
                Name = topicName,
                NumPartitions = numPartitions,
                ReplicationFactor = replicationFactor
            };

            await client.CreateTopicsAsync([topicSpecification]);
            Console.WriteLine($"Topic '{topicName}' created successfully. \u2728 ");
        }
        catch (CreateTopicsException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    
    /// <summary>
    /// Sends a message containing an OrderShippedNotification to the specified Kafka topic.
    /// </summary>
    /// <param name="topicName">The name of the Kafka topic to send the message to.</param>
    /// <param name="notificationEvent">The OrderShippedNotification object containing the message data.</param>
    public async Task SendMessageAsync(string topicName, OrderShippedNotification notificationEvent)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = "localhost:9094",
        };

        using var producer = new ProducerBuilder<Null, string>(config).Build();
        try
        {
            var value = JsonSerializer.Serialize(notificationEvent);
            var result = await producer.ProduceAsync(topicName, new Message<Null, string>
            {
                Value = value
            });

            Console.WriteLine($"The message OrderCode:{notificationEvent.OrderCode} sent to Partition:{result.Partition} and Offset:{result.Offset} 🚀");
        }
        catch (ProduceException<string, string> ex)
        {
            Console.WriteLine($"Failed to deliver message: {ex.Error.Reason}");
        }
    }
}