using KafkaDemo.Producer;

var topicName = "topic-" + DateTime.Now.ToString("yyyyMMdd");
var kafkaService = new KafkaService();
await kafkaService.CreateTopic(topicName);
await kafkaService.SendMessage(topicName, "key-hello", "value-world");

// To remove a topic:
// await kafkaService.RemoveTopic(topicName);

// To list topics:
// await kafkaService.ListTopics();

// To see topic partitions:
// await kafkaService.GetTopicPartitions(topicName);