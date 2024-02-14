namespace SharedEvents
{
    public record OrderCreatedEvent
    {
        public string OrderCode { get; init; } = default!;
        public decimal TotalPrice { get; init; }
    }
}