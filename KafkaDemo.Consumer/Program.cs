using KafkaDemo.Consumer;

Console.WriteLine("Consumer started!");

var cts = new CancellationTokenSource();
try
{
    var kafkaService = new KafkaService();
    var topicName = "topic-" + DateTime.Now.ToString("yyyyMMdd");
    await kafkaService.ConsumeMessages(topicName, cts.Token);
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred: {ex.Message}");
}
finally
{
    cts.Cancel();
}
