namespace AcklenAvenue.Queueing
{
    public interface IMessageReceived<TMessage>
    {
        TMessage Message { get; set; }
    }
}