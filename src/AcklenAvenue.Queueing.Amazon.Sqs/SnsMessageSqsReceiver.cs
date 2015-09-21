using AcklenAvenue.Queueing.Amazon.Sqs.Builder;
using Amazon.SQS.Model;

namespace AcklenAvenue.Queueing.Amazon.Sqs
{
    public class SnsMessageSqsReceiver<TMessage> : AWSSqsReceiverBase<TMessage>
    {
        readonly IMessageDeserializer _deserializer;

        public SnsMessageSqsReceiver(
            IAwsConfig awsConfig, string serviceUrl, string queueUrl, IMessageDeserializer deserializer)
            : base(awsConfig, serviceUrl, queueUrl)
        {
            _deserializer = deserializer;
        }

        protected override SqsMessageReceived<TMessage> CreateResponseMessage(Message message)
        {
            var snsMessage = _deserializer.Deserialize<SnsMessageAdapter>(message.Body);

            var extractedMessage = _deserializer.Deserialize<TMessage>(snsMessage.Message);

            return new SqsMessageReceived<TMessage>
                   {
                       Id = message.MessageId,
                       Message = extractedMessage,
                       ReceiptHandle = message.ReceiptHandle
                   };
        }

        public class SnsMessageAdapter
        {
            public string Message { get; set; }
        }
    }
}