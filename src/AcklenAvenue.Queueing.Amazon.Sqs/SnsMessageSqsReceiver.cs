using Amazon.SQS.Model;

using Newtonsoft.Json;

namespace AcklenAvenue.Queueing.Amazon.Sqs
{
    public class SnsMessageSqsReceiver<TMessage> : AWSSqsReceiverBase<TMessage>
    {
        public SnsMessageSqsReceiver(
            string awsAccessKeyId, string awsSecretAccessKey, string serviceUrl, string queueUrl)
            : base(awsAccessKeyId, awsSecretAccessKey, serviceUrl, queueUrl)
        {
        }

        protected override SqsMessageReceived<TMessage> CreateResponseMessage(Message message)
        {
            var snsMessage = JsonConvert.DeserializeObject<SnsMessageAdapter>(message.Body);

            var extractedMessage = JsonConvert.DeserializeObject<TMessage>(snsMessage.Message);

            return new SqsMessageReceived<TMessage> { Id = message.MessageId, Message = extractedMessage };
        }

        public class SnsMessageAdapter
        {
            public string Message { get; set; }
        }
    }
}