namespace KafkaDemo.Consumer.Events;

public record OrderShippedNotification
{
    public string OrderCode { get; init; } = null!;
    public DateTime ShippedDate { get; init; } 
    public string CarrierName { get; init; } = null!;
    public string TrackingNumber { get; init; } = null!;
}