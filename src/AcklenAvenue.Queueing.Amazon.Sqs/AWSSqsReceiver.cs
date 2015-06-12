using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Amazon.SQS;
using Amazon.SQS.Model;

using Newtonsoft.Json;

namespace AcklenAvenue.Queueing.Amazon.Sqs
{
    public class AWSSqsReceiver<TMessage> : IMessageReceiver<TMessage>
    {
        public AWSSqsReceiver(string awsAccessKeyId, string awsSecretAccessKey, string serviceUrl, string queueUrl)
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

                ReceiveMessageResponse response = sqsClient.ReceiveMessage(receiveMessageRequest);

                var messages = new List<SqsMessageReceived<TMessage>>();
                foreach (Message message in response.Messages)
                {
                    string stringType = message.MessageAttributes["Type"].StringValue;

                    string domain = stringType.Split('.').First();
                    IEnumerable<Assembly> assemblies =
                        AppDomain.CurrentDomain.GetAssemblies().Where(assembly => assembly.FullName.StartsWith(domain));
                    List<Type> selectMany = assemblies.SelectMany(x => x.ExportedTypes).ToList();
                    Type type = selectMany.FirstOrDefault(x => x.FullName.EndsWith(stringType));

                    object deserializeObject = JsonConvert.DeserializeObject(message.Body, type);
                    var messageReceived = new SqsMessageReceived<TMessage>
                                              {
                                                  ReceiptHandle = message.ReceiptHandle,
                                                  Message = (TMessage)deserializeObject
                                              };

                    messages.Add(messageReceived);
                }
                return messages;
            }
        }
    }
}