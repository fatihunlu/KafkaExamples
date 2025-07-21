using KafkaDemo.Consumer;


try
{
    await new FollowEventConsumer().ConsumeAsync();
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred: {ex.Message}");
}