namespace AcklenAvenue.Queueing.Amazon.Sqs
{
    public interface IMessageSerializer
    {
        string Serialize(object message);
    }
}