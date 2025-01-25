using KafkaDemo.Producer;
using KafkaDemo.Producer.Events;

var kafkaService = new KafkaService();

var topicName = "order-shipped-notifications";
await kafkaService.CreateTopicAsync(topicName);

while (true)
{
    await kafkaService.SendMessageAsync(
        topicName,
        new OrderShippedNotification
        {
            OrderCode = Guid.NewGuid().ToString(),
            ShippedDate = DateTime.UtcNow,
            CarrierName = "random-carrier",
            TrackingNumber = $"TRK-{Guid.NewGuid().ToString("N").Substring(0, 10)}"
        }
    );
    await Task.Delay(1000);
}
