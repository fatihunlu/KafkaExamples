using KafkaDemo.DLQEventHandler;
using MongoDB.Driver;

try
{
    var connectionString = "mongodb://root:example@localhost:27017";
    var client = new MongoClient(connectionString);

    await new DLQEventHandler(client).StartAsync();
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred: {ex.Message}");
}