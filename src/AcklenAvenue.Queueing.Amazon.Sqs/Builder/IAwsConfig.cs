using Amazon.Runtime;
using Amazon.SQS;

namespace AcklenAvenue.Queueing.Amazon.Sqs.Builder
{
    public interface IAwsConfig
    {
        TClient CreateClientRequest<TClient>(ClientConfig amazonSqsConfig) where TClient : AmazonServiceClient;
    }
}