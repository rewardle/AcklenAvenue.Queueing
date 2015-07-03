namespace AcklenAvenue.Queueing.Amazon.Sqs
{
    public interface IMessageDeserializer
    {
        T Deserialize<T>(string json);
    }
}