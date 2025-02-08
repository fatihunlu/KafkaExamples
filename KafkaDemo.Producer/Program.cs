using KafkaDemo.Producer;
using KafkaDemo.Producer.Events;

var kafkaService = new KafkaService();
const string topicName = "socialmedia-user-follow";
await kafkaService.CreateTopicAsync(topicName);

var rand = new Random();
while (true)
{
    await kafkaService.SendMessageAsync(
        topicName,
        new FollowerEvent
        {
            FollowerId = rand.Next(1, 10),
            FolloweeId = rand.Next(20, 30),
        }
    );
    await Task.Delay(1000);
}