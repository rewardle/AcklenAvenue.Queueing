using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using AWS = Amazon;

namespace AcklenAvenue.Queueing.Amazon.Sqs
{
    public class AWSSnsSender<TMessage> : IMessageSender<TMessage>
    {
        readonly IMessageSerializer _serializer;

        public AWSSnsSender(string accessKey, string secretKey, string serviceUrl, string topicArn, IMessageSerializer serializer)
        {
            _serializer = serializer;
            AccessKey = accessKey;
            SecretKey = secretKey;
            ServiceUrl = serviceUrl;
            TopicArn = topicArn;
        }

        public string AccessKey { get; set; }

        public string SecretKey { get; set; }

        public string ServiceUrl { get; set; }

        public string TopicArn { get; set; }

        public ISendResponse Send(TMessage message)
        {
            var config = new AmazonSimpleNotificationServiceConfig
                         {
                             ServiceURL =
                                 ServiceUrl
                         };

            PublishResponse response;
            using (
                var sns = new AmazonSimpleNotificationServiceClient(
                    AccessKey, SecretKey, config))
            {
                string messageToSend = _serializer.Serialize(message);
                var publishRequest = new PublishRequest(TopicArn, messageToSend);

                
                response = sns.Publish(publishRequest);
            }

            return new SendResponse(response.MessageId);
        }
    }
}