using System.Threading.Tasks;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace AcklenAvenue.Queueing.Amazon.Sqs
{
    public class AWSSqsSender<TMessage> : IMessageSender<TMessage>
    {
        readonly IMessageSerializer _serializer;

        public AWSSqsSender(string awsAccessKeyId, string awsSecretAccessKey, string serviceUrl, string queueUrl, IMessageSerializer serializer)
        {
            _serializer = serializer;
            AwsAccessKeyId = awsAccessKeyId;
            SecretAccessKey = awsSecretAccessKey;
            ServiceUrl = serviceUrl;
            QueueUrl = queueUrl;
            Delay = 0;
        }

        public string AwsAccessKeyId { get; set; }

        public string SecretAccessKey { get; set; }

        public string ServiceUrl { get; set; }

        public string QueueUrl { get; set; }

        public int Delay { get; set; }

        public async Task<ISendResponse> Send(TMessage message)
        {
            var amazonSqsConfig = new AmazonSQSConfig {ServiceURL = ServiceUrl};

            using (var sqsClient = new AmazonSQSClient(AwsAccessKeyId, SecretAccessKey, amazonSqsConfig))
            {
                string body = _serializer.Serialize(message);
                string typeOfMessage = (typeof (TMessage)).FullName;
                var sendMessageRequest = new SendMessageRequest(QueueUrl, body) {DelaySeconds = Delay};
                var messageAttributeValue = new MessageAttributeValue
                                            {
                                                StringValue = typeOfMessage,
                                                DataType = "String"
                                            };

                sendMessageRequest.MessageAttributes.Add("Type", messageAttributeValue);

                SendMessageResponse response = await sqsClient.SendMessageAsync(sendMessageRequest);

                return new SendResponse(response.MessageId);
            }
        }
    }
}