using Amazon.SQS;
using Amazon.SQS.Model;

using Machine.Specifications;

namespace AcklenAvenue.Queueing.Amazon.Sqs.Specs.Integration
{
    public class given_sqs_client_context
    {
        protected static string acces;

        protected static string scrt;

        protected static string ServiceUrl;

        protected static string Queuename;

        protected static AmazonSQSConfig AmazonSqsConfig;

        protected static AmazonSQSClient Sqs;

        protected static CreateQueueResponse CreateQueueResponse;

        //Cleanup after = () => Sqs.DeleteQueue(new DeleteQueueRequest(CreateQueueResponse.QueueUrl));

        Establish context = () =>
            {
                acces = "acces";
                scrt = "key+";
                ServiceUrl = "http://sqs.us-west-2.amazonaws.com";
                Queuename = "TestQueue";

                AmazonSqsConfig = new AmazonSQSConfig { ServiceURL = ServiceUrl };

                Sqs = new AmazonSQSClient( AmazonSqsConfig);

                CreateQueueResponse = Sqs.CreateQueue(Queuename);
            };
    }
}