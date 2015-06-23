namespace AcklenAvenue.Queueing.Amazon.Sqs
{
    public class SqsMessageReceived<TMessage> : IMessageReceived<TMessage>
    {
        public string ReceiptHandle { get; set; }

        public string Id { get; set; }

        public TMessage Message { get; set; }
    }
}