using System.Collections.Generic;

using Amazon.SQS;
using Amazon.SQS.Model;

namespace AcklenAvenue.Queueing.Amazon.Sqs
{
    public abstract class AWSSqsReceiverBase<TMessage> : IMessageReceiver<TMessage>
    {
        protected AWSSqsReceiverBase(string awsAccessKeyId, string awsSecretAccessKey, string serviceUrl, string queueUrl)
        {
            AwsAccessKeyId = awsAccessKeyId;
            AwsSecretAccessKey = awsSecretAccessKey;
            ServiceUrl = serviceUrl;
            QueueUrl = queueUrl;
            MaxNumberOfMessages = 1;
            VisibilityTimeOut = 300;
        }

        public string AwsAccessKeyId { get; set; }

        public string AwsSecretAccessKey { get; set; }

        public string ServiceUrl { get; set; }

        public string QueueUrl { get; set; }

        public int MaxNumberOfMessages { get; set; }

        public int VisibilityTimeOut { get; set; }

        public IEnumerable<IMessageReceived<TMessage>> Receive()
        {
            var amazonSqsConfig = new AmazonSQSConfig { ServiceURL = ServiceUrl };

            using (var sqsClient = new AmazonSQSClient(AwsAccessKeyId, AwsSecretAccessKey, amazonSqsConfig))
            {
                var receiveMessageRequest = new ReceiveMessageRequest(QueueUrl)
                                                {
                                                    MaxNumberOfMessages =
                                                        MaxNumberOfMessages,
                                                    VisibilityTimeout =
                                                        VisibilityTimeOut
                                                };
                receiveMessageRequest.MessageAttributeNames.Add("All");

                ReceiveMessageResponse response = sqsClient.ReceiveMessageAsync(receiveMessageRequest).Result;

                var messages = new List<SqsMessageReceived<TMessage>>();
                foreach (Message message in response.Messages)
                {
                    SqsMessageReceived<TMessage> messageReceived = CreateResponseMessage(message);

                    messages.Add(messageReceived);
                }
                return messages;
            }
        }

        protected abstract SqsMessageReceived<TMessage> CreateResponseMessage(Message message);
    }
}