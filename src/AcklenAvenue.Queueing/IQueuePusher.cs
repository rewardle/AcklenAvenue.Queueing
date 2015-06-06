namespace AcklenAvenue.Queueing
{
    public interface IQueuePusher<in T>
    {
        void Push(T @event);
    }
}