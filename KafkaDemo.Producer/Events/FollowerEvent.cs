namespace KafkaDemo.Producer.Events;

public class FollowerEvent
{
    public int FollowerId { get; set; }
    public int FolloweeId { get; set; }
}
