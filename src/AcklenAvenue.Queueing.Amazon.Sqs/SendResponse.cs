namespace AcklenAvenue.Queueing.Amazon.Sqs
{
    public class SendResponse : ISendResponse
    {
        public SendResponse(string messageId)
        {
            MessageId = messageId;
        }

        public string MessageId { get; set; }
    }
}