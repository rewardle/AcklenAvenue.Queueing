namespace AcklenAvenue.Queueing.Amazon.Sqs
{
    public class SubcriptionMessage : ISubscribeReponseMessage
    {
        public SubcriptionMessage(string message)
        {
            Message = message;
        }

        public string Message { get; set; }
    }
}