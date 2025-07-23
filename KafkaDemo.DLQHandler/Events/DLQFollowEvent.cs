namespace KafkaDemo.DLQHandler.Events;

public class DLQFollowEvent
{
    public string OriginalPayload { get; set; }
    public string ErrorMessage { get; set; }
    public DateTime FailedAt { get; set; }
    public string Topic { get; set; }
    public string Partition { get; set; }
    public long Offset { get; set; }
}
