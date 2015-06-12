namespace AcklenAvenue.Queueing.Amazon.Sqs
{
    public class SqsMessageReceived<TMessage> : IMessageReceived<TMessage>
    {
        public TMessage Message { get; set; }

        public string ReceiptHandle { get; set; }
    }
}