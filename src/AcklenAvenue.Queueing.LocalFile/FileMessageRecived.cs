namespace AcklenAvenue.Queueing.LocalFile
{
    public class FileMessageRecived<T> : IMessageReceived<T>
    {
        public FileMessageRecived(T message)
        {
            Message = message;
        }

        public string Id { get; set; }

        public T Message { get; set; }
    }
}