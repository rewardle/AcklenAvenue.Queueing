using System;
using AcklenAvenue.Queueing.Amazon.Sqs.Builder;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace AcklenAvenue.Queueing.Amazon.Sqs
{
    public class SqsMessageDeleter<TMessage> : IMessageDeleter<TMessage>
    {
        readonly IAwsConfig _awsConfig;

        public SqsMessageDeleter(
            IAwsConfig awsConfig, string serviceUrl, string queueUrl)
        {
            _awsConfig = awsConfig;
            ServiceUrl = serviceUrl;
            QueueUrl = queueUrl;
        }

        public string ServiceUrl { get; set; }
        public string QueueUrl { get; set; }

        public async void Delete(IMessageReceived<TMessage> messageReceived)
        {
            var amazonSqsConfig = new AmazonSQSConfig {ServiceURL = ServiceUrl};
            var msg = messageReceived as SqsMessageReceived<TMessage>;
            if (msg == null)
            {
                throw new Exception(string.Format("The message you send is not form AWS SQS"));
            }
            using (var sqsClient = _awsConfig.CreateAwsClient<AmazonSQSClient>(amazonSqsConfig))
            {
                var delete =
                    await sqsClient.DeleteMessageAsync(new DeleteMessageRequest(QueueUrl, msg.ReceiptHandle));
                var d = delete.ContentLength;
            }
        }
    }
}