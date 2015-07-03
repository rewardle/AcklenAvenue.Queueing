namespace AcklenAvenue.Queueing.Amazon.Sqs.Builder
{
    public interface IQueueConfiguration
    {
        IQueueConfiguration SetMaxNumberOfMessages(int max);

        BusBuilder SetVisibilityTimeOut(int timeOut);
    }
}