namespace AcklenAvenue.Queueing.Amazon.Sqs
{
    public interface IQueueSubcriber
    {
        ISubscribeReponseMessage Subscribe(string queueUrl);
    }
}