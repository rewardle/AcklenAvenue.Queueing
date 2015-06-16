using Amazon.SQS.Model;

using Newtonsoft.Json;

namespace AcklenAvenue.Queueing.Amazon.Sqs
{
    public class NormalAWSSqsReceiver<TMessage> : AWSSqsReceiverBase<TMessage>
    {
        public NormalAWSSqsReceiver(
            string awsAccessKeyId, string awsSecretAccessKey, string serviceUrl, string queueUrl)
            : base(awsAccessKeyId, awsSecretAccessKey, serviceUrl, queueUrl)
        {
        }

        protected override SqsMessageReceived<TMessage> CreateResponseMessage(Message message)
        {
            var deserializeObject = JsonConvert.DeserializeObject<TMessage>(message.Body);
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