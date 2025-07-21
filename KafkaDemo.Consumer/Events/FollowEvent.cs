namespace KafkaDemo.Consumer.Events;

public class FollowEvent
{
    public string FollowerId { get; set; } = default!;
    public string FolloweeId { get; set; } = default!;
    public long Timestamp { get; set; }
}
