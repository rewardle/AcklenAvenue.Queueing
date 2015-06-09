using Amazon.SQS;
using Amazon.SQS.Model;

namespace AcklenAvenue.Queueing.Amazon.Sqs
{
    public class SqsMessageDeleter<TMessage> : IMessageDeleter<TMessage>
    {
        public SqsMessageDeleter(string awsAccessKeyId, string awsSecretAccessKey, string serviceUrl, string queueUrl)
        {
            AwsAccessKeyId = awsAccessKeyId;
            AwsSecretAccessKey = awsSecretAccessKey;
            ServiceUrl = serviceUrl;
            QueueUrl = queueUrl;
        }

        public string AwsAccessKeyId { get; set; }

        public string AwsSecretAccessKey { get; set; }

        public string ServiceUrl { get; set; }

        public string QueueUrl { get; set; }

        public async void Delete(IMessageReceived<TMessage> messageReceived)
        {
            var amazonSqsConfig = new AmazonSQSConfig { ServiceURL = ServiceUrl };

            using (var sqsClient = new AmazonSQSClient(AwsAccessKeyId, AwsSecretAccessKey, amazonSqsConfig))
            {
                DeleteMessageResponse delete = await 
                    sqsClient.DeleteMessageAsync(new DeleteMessageRequest(QueueUrl, messageReceived.ReceiptHandle));
                long d = delete.ContentLength;
            }
        }
    }
}