using System.Collections.Generic;

using Amazon.Auth.AccessControlPolicy;
using Amazon.Auth.AccessControlPolicy.ActionIdentifiers;
using Amazon.SQS;
using Amazon.SQS.Model;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;

using AWS = Amazon;

namespace AcklenAvenue.Queueing.Amazon.Sqs
{
    public class AWSSnsSubcriber : IQueueSubcriber
    {
        public AWSSnsSubcriber(
            string accessKey, string secretKey, string snsServicesUrl, string sqsServiceUrl, string topicArn)
        {
            AccessKey = accessKey;
            SecretKey = secretKey;
            SnsServiceUrl = snsServicesUrl;
            SqsServiceUrl = sqsServiceUrl;
            TopicArn = topicArn;
        }

        public string AccessKey { get; set; }

        public string SecretKey { get; set; }

        public string SnsServiceUrl { get; set; }

        public string SqsServiceUrl { get; set; }

        public string TopicArn { get; set; }

        public ISubscribeReponseMessage Subscribe(string queueUrl)
        {
            var sqsConfig = new AmazonSQSConfig { ServiceURL = SqsServiceUrl };
            var snsConfig = new AmazonSimpleNotificationServiceConfig { ServiceURL = SnsServiceUrl };

            using (var sqsClient = new AmazonSQSClient(AccessKey, SecretKey, sqsConfig))
            {
                var attributesRequest = new GetQueueAttributesRequest(queueUrl, new List<string> { "QueueArn" });

                GetQueueAttributesResponse attributesResponse = sqsClient.GetQueueAttributes(attributesRequest);

                using (var snsClient = new AmazonSimpleNotificationServiceClient(AccessKey, SecretKey, snsConfig))
                {
                    SubscribeResponse subribeResonse =
                        snsClient.Subscribe(new SubscribeRequest(TopicArn, "sqs", attributesResponse.QueueARN));
                }

                var actions = new ActionIdentifier[2];
                actions[0] = SQSActionIdentifiers.SendMessage;
                actions[1] = SQSActionIdentifiers.ReceiveMessage;
                Policy sqsPolicy =
                    new Policy().WithStatements(
                        new Statement(Statement.StatementEffect.Allow).WithPrincipals(Principal.AllUsers)
                                                                      .WithResources(
                                                                          new Resource(attributesResponse.QueueARN))
                                                                      .WithConditions(
                                                                          ConditionFactory.NewSourceArnCondition(
                                                                              TopicArn))
                                                                      .WithActionIdentifiers(actions));
                var setQueueAttributesRequest = new SetQueueAttributesRequest();

                var attributes = new Dictionary<string, string> { { "Policy", sqsPolicy.ToJson() } };
                var attRequest = new SetQueueAttributesRequest(attributesRequest.QueueUrl, attributes);

                sqsClient.SetQueueAttributes(attRequest);

                return new SubcriptionMessage("Ok");
            }
        }
    }
}