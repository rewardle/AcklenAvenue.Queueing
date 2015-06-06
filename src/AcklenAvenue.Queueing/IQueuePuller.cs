namespace AcklenAvenue.Queueing
{
    public interface IQueuePuller<out T>
    {
        T Pull();
    }
}