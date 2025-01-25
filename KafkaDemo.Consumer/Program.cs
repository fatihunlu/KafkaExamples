using KafkaDemo.Consumer;

Console.WriteLine("Consumer started!");

var cts = new CancellationTokenSource();
try
{
    var kafkaService = new KafkaService();
    
    var consumerGroup = args.Length > 0 ? args[0] : "default-consumer-group";
    var topicName = "order-shipped-notifications";
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
