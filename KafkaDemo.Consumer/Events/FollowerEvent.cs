namespace KafkaDemo.Consumer.Events;

public class FollowerEvent
{
    public int FollowerId { get; set; }
    public int FolloweeId { get; set; }
}
