using Confluent.Kafka;
using Confluent.Kafka.Admin;

namespace KafkaDemo.Producer;

public class KafkaService
{
    public async Task CreateTopic(string topicName)
    {
        using var client = new AdminClientBuilder(new AdminClientConfig()
        {
            BootstrapServers = "localhost:9094",
        }).Build();

        try
        {
            await client.CreateTopicsAsync([
                new TopicSpecification(){ Name = topicName }
            ]);
            
            Console.WriteLine($"{topicName} is successfully created \u2705 ");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    public async Task RemoveTopic(string topicName)
    {
        using var client = new AdminClientBuilder(new AdminClientConfig()
        {
            BootstrapServers = "localhost:9094",
        }).Build();
        
        try
        {
            await client.DeleteTopicsAsync(new[] { topicName });
            Console.WriteLine($"Topic '{topicName}' has been deleted successfully. \u2705 ");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    } 
    public async Task ListTopics()
    {
        using var client = new AdminClientBuilder(new AdminClientConfig()
        {
            BootstrapServers = "localhost:9094",
        }).Build();

        try
        {
            var metadata = client.GetMetadata(TimeSpan.FromSeconds(10));
            
            Console.WriteLine("Topics in the cluster:");
            foreach (var topic in metadata.Topics)
            {
                Console.WriteLine($" 🗂️ {topic.Topic}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while listing topics: {ex.Message}");
        }
    }
    
    public async Task SendMessage(string topicName, string key, string value)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = "localhost:9094"
        };

        using var producer = new ProducerBuilder<string, string>(config).Build();

        try
        {
            var result = await producer.ProduceAsync(topicName, new Message<string, string>
            {
                Key = key,
                Value = value
            });

            Console.WriteLine($"\ud83d\udce4 Message delivered to {result.TopicPartitionOffset}");
        }
        catch (ProduceException<string, string> ex)
        {
            Console.WriteLine($"Failed to deliver message: {ex.Error.Reason}");
        }
    }
    
    public async Task GetTopicPartitions(string topicName)
    {
        using var adminClient = new AdminClientBuilder(new AdminClientConfig
        {
            BootstrapServers = "localhost:9094"
        }).Build();

        try
        {
            var metadata = adminClient.GetMetadata(topicName, TimeSpan.FromSeconds(10));

            Console.WriteLine($"Topic: {topicName}");
            Console.WriteLine($"Partition Count: {metadata.Topics[0].Partitions.Count}");

            foreach (var partition in metadata.Topics[0].Partitions)
            {
                Console.WriteLine($"Partition ID: {partition.PartitionId}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}