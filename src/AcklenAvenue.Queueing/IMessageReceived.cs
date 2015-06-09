namespace AcklenAvenue.Queueing
{
    public interface IMessageReceived<TMessage>
    {
        string ReceiptHandle { get; set; }

        TMessage Message { get; set; }
    }
}