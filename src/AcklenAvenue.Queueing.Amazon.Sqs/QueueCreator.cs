using AcklenAvenue.Queueing.Amazon.Sqs.Builder;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace AcklenAvenue.Queueing.Amazon.Sqs
{
    public class QueueCreator : IQueueCreator
    {
        readonly IAwsConfig _awsConfig;

        public QueueCreator(IAwsConfig awsConfig, string serviceUrl)
        {
            _awsConfig = awsConfig;

            ServiceUrl = serviceUrl;
        }

        public string ServiceUrl { get; set; }

        public string CreateQueue(string queueName)
        {
            var amazonSqsConfig = new AmazonSQSConfig {ServiceURL = ServiceUrl};

            using (var sqsClient = _awsConfig.CreateAwsClient<AmazonSQSClient>(amazonSqsConfig))
            {
                var response = sqsClient.CreateQueue(new CreateQueueRequest(queueName));

                return response.QueueUrl;
            }
        }
    }
}