using System.Collections.Generic;
using System.Threading.Tasks;
using AcklenAvenue.Queueing.Amazon.Sqs.Builder;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace AcklenAvenue.Queueing.Amazon.Sqs
{
    public abstract class AWSSqsReceiverBase<TMessage> : IMessageReceiver<TMessage>
    {
        readonly IAwsConfig _awsConfig;

        protected AWSSqsReceiverBase(IAwsConfig awsConfig, string serviceUrl, string queueUrl)
        {
            _awsConfig = awsConfig;
            ServiceUrl = serviceUrl;
            QueueUrl = queueUrl;
            MaxNumberOfMessages = 1;
            VisibilityTimeOut = 300;
        }

        public string ServiceUrl { get; set; }
        public string QueueUrl { get; set; }
        public int MaxNumberOfMessages { get; set; }
        public int VisibilityTimeOut { get; set; }

        public async Task<IEnumerable<IMessageReceived<TMessage>>> Receive()
        {
            var amazonSqsConfig = new AmazonSQSConfig {ServiceURL = ServiceUrl};

            using (var sqsClient = _awsConfig.CreateAwsClient<AmazonSQSClient>(amazonSqsConfig))
            {
                var receiveMessageRequest = new ReceiveMessageRequest(QueueUrl)
                                            {
                                                MaxNumberOfMessages =
                                                    MaxNumberOfMessages,
                                                VisibilityTimeout =
                                                    VisibilityTimeOut
                                            };
                receiveMessageRequest.MessageAttributeNames.Add("All");

                var response = await sqsClient.ReceiveMessageAsync(receiveMessageRequest);

                var messages = new List<SqsMessageReceived<TMessage>>();
                foreach (var message in response.Messages)
                {
                    var messageReceived = CreateResponseMessage(message);

                    messages.Add(messageReceived);
                }
                return messages;
            }
        }

        protected abstract SqsMessageReceived<TMessage> CreateResponseMessage(Message message);
    }
}