namespace AcklenAvenue.Queueing
{
    public interface IMessageReceived<TMessage>
    {
        string Id { get; set; }

        TMessage Message { get; set; }
    }
}