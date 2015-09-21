using AcklenAvenue.Queueing.Amazon.Sqs.Builder;
using Amazon.SQS.Model;

namespace AcklenAvenue.Queueing.Amazon.Sqs
{
    public class NormalAWSSqsReceiver<TMessage> : AWSSqsReceiverBase<TMessage>
    {
        readonly IMessageDeserializer _deserialzer;

        public NormalAWSSqsReceiver(
            IAwsConfig awsConfig, string serviceUrl, string queueUrl, IMessageDeserializer deserialzer)
            : base(awsConfig, serviceUrl, queueUrl)
        {
            _deserialzer = deserialzer;
        }

        

        protected override SqsMessageReceived<TMessage> CreateResponseMessage(Message message)
        {
            var deserializeObject = _deserialzer.Deserialize<TMessage>(message.Body);
            var messageReceived = new SqsMessageReceived<TMessage>
                                  {
                                      Id = message.MessageId,
                                      ReceiptHandle = message.ReceiptHandle,
                                      Message = deserializeObject
                                  };
            return messageReceived;
        }
    }
}