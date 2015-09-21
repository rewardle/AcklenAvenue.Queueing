using System.Threading.Tasks;
using AcklenAvenue.Queueing.Amazon.Sqs.Builder;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using AWS = Amazon;

namespace AcklenAvenue.Queueing.Amazon.Sqs
{
    public class AWSSnsSender<TMessage> : IMessageSender<TMessage>
    {
        readonly IAwsConfig _awsConfig;
        readonly IMessageSerializer _serializer;

        public AWSSnsSender(IAwsConfig awsConfig, string serviceUrl, string topicArn, IMessageSerializer serializer)
        {
            _awsConfig = awsConfig;
            _serializer = serializer;
            ServiceUrl = serviceUrl;
            TopicArn = topicArn;
        }

        public string ServiceUrl { get; set; }
        public string TopicArn { get; set; }

        public async Task<ISendResponse> Send(TMessage message)
        {
            var config = new AmazonSimpleNotificationServiceConfig
                         {
                             ServiceURL =
                                 ServiceUrl
                         };

            PublishResponse response;
            using (
                var sns = _awsConfig.CreateAwsClient<AmazonSimpleNotificationServiceClient>(config))
            {
                var messageToSend = _serializer.Serialize(message);
                var publishRequest = new PublishRequest(TopicArn, messageToSend);

                response = await sns.PublishAsync(publishRequest);
            }

            return new SendResponse(response.MessageId);
        }
    }
}