using Amazon.SQS;
using Amazon.SQS.Model;

namespace AcklenAvenue.Queueing.Amazon.Sqs
{
    public class QueueCreator : IQueueCreator
    {
        public QueueCreator(string awsAccessKeyId, string awsSecretAccessKey, string serviceUrl)
        {
            AwsAccessKeyId = awsAccessKeyId;
            AwsSecretAccessKey = awsSecretAccessKey;
            ServiceUrl = serviceUrl;
        }

        public string AwsAccessKeyId { get; set; }

        public string AwsSecretAccessKey { get; set; }

        public string ServiceUrl { get; set; }

        public string CreateQueue(string queueName)
        {
            var amazonSqsConfig = new AmazonSQSConfig { ServiceURL = ServiceUrl };

            using (var sqsClient = new AmazonSQSClient(AwsAccessKeyId, AwsSecretAccessKey, amazonSqsConfig))
            {
                CreateQueueResponse response = sqsClient.CreateQueue(new CreateQueueRequest(queueName));

                return response.QueueUrl;
            }
        }
    }
}