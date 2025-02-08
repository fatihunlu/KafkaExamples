using KafkaDemo.Consumer;
using MongoDB.Driver;

Console.WriteLine("Consumer started!");

// Best practice is to load the connection string from environment variables, but it's hardcoded here for simplicity in the tutorial.
var connectionString = "mongodb://root:example@localhost:27017";
var client = new MongoClient(connectionString);

var cts = new CancellationTokenSource();
try
{
    var kafkaService = new KafkaService(client);
    var consumerGroup = "default-consumer-group";
    var topicName = "socialmedia-user-follow";
    await kafkaService.ConsumeMessages(topicName, consumerGroup, cts.Token);
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred: {ex.Message}");
}
finally
{
    cts.Cancel();
}
