namespace AcklenAvenue.Queueing
{
    public interface IQueueCreator
    {
        string CreateQueue(string queueName);
    }
}