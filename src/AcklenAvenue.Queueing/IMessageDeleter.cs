namespace AcklenAvenue.Queueing
{
    public interface IMessageDeleter<TMessage>
    {
        void Delete(IMessageReceived<TMessage> messageReceived);
    }
}