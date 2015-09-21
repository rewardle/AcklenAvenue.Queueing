using System.Collections.Generic;
using AcklenAvenue.Queueing.Amazon.Sqs.Builder;
using Amazon.Auth.AccessControlPolicy;
using Amazon.Auth.AccessControlPolicy.ActionIdentifiers;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Amazon.SQS;
using Amazon.SQS.Model;
using AWS = Amazon;

namespace AcklenAvenue.Queueing.Amazon.Sqs
{
    public class AWSSnsSubcriber : IQueueSubcriber
    {
        readonly IAwsConfig _awsConfig;

        public AWSSnsSubcriber(
            IAwsConfig awsConfig, string snsServicesUrl, string sqsServiceUrl, string topicArn)
        {
            _awsConfig = awsConfig;

            SnsServiceUrl = snsServicesUrl;
            SqsServiceUrl = sqsServiceUrl;
            TopicArn = topicArn;
        }

        public string SnsServiceUrl { get; set; }
        public string SqsServiceUrl { get; set; }
        public string TopicArn { get; set; }

        public ISubscribeReponseMessage Subscribe(string queueUrl)
        {
            var sqsConfig = new AmazonSQSConfig {ServiceURL = SqsServiceUrl};
            var snsConfig = new AmazonSimpleNotificationServiceConfig {ServiceURL = SnsServiceUrl};

            using (var sqsClient = _awsConfig.CreateAwsClient<AmazonSQSClient>(sqsConfig))
            {
                var attributesRequest = new GetQueueAttributesRequest(queueUrl, new List<string> {"QueueArn"});

                var attributesResponse = sqsClient.GetQueueAttributes(attributesRequest);

                using (var snsClient = _awsConfig.CreateAwsClient<AmazonSimpleNotificationServiceClient>(snsConfig))
                {
                    var subribeResonse =
                        snsClient.Subscribe(new SubscribeRequest(TopicArn, "sqs", attributesResponse.QueueARN));
                }

                var actions = new ActionIdentifier[2];
                actions[0] = SQSActionIdentifiers.SendMessage;
                actions[1] = SQSActionIdentifiers.ReceiveMessage;
                var sqsPolicy =
                    new Policy().WithStatements(
                        new Statement(Statement.StatementEffect.Allow).WithPrincipals(Principal.AllUsers)
                            .WithResources(
                                new Resource(attributesResponse.QueueARN))
                            .WithConditions(
                                ConditionFactory.NewSourceArnCondition(
                                    TopicArn))
                            .WithActionIdentifiers(actions));
                var setQueueAttributesRequest = new SetQueueAttributesRequest();

                var attributes = new Dictionary<string, string> {{"Policy", sqsPolicy.ToJson()}};
                var attRequest = new SetQueueAttributesRequest(attributesRequest.QueueUrl, attributes);

                sqsClient.SetQueueAttributes(attRequest);

                return new SubcriptionMessage("Ok");
            }
        }
    }
}