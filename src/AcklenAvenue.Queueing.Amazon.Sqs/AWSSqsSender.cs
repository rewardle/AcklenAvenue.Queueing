using System.Threading.Tasks;
using AcklenAvenue.Queueing.Amazon.Sqs.Builder;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace AcklenAvenue.Queueing.Amazon.Sqs
{
    public class AWSSqsSender<TMessage> : IMessageSender<TMessage>
    {
        readonly IAwsConfig _awsConfig;
        readonly IMessageSerializer _serializer;

        public AWSSqsSender(IAwsConfig awsConfig, string serviceUrl, string queueUrl, IMessageSerializer serializer)
        {
            _awsConfig = awsConfig;
            _serializer = serializer;
            ServiceUrl = serviceUrl;
            QueueUrl = queueUrl;
            Delay = 0;
        }

        public string ServiceUrl { get; set; }
        public string QueueUrl { get; set; }
        public int Delay { get; set; }

        public async Task<ISendResponse> Send(TMessage message)
        {
            var amazonSqsConfig = new AmazonSQSConfig {ServiceURL = ServiceUrl};

            using (var sqsClient = _awsConfig.CreateAwsClient<AmazonSQSClient>(amazonSqsConfig))
            {
                var body = _serializer.Serialize(message);
                var typeOfMessage = (typeof (TMessage)).FullName;
                var sendMessageRequest = new SendMessageRequest(QueueUrl, body) {DelaySeconds = Delay};
                var messageAttributeValue = new MessageAttributeValue
                                            {
                                                StringValue = typeOfMessage,
                                                DataType = "String"
                                            };

                sendMessageRequest.MessageAttributes.Add("Type", messageAttributeValue);

                var response = await sqsClient.SendMessageAsync(sendMessageRequest);

                return new SendResponse(response.MessageId);
            }
        }
    }
}