namespace KafkaDemo.Consumer.Events;

public class DLQFollowEvent
{
    public string Key { get; set; }
    public string OriginalPayload { get; set; }
    public string ErrorMessage { get; set; }
    public DateTime FailedAt { get; set; }
}
