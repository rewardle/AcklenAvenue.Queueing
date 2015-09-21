using Amazon.Runtime;
using Amazon.SQS;

namespace AcklenAvenue.Queueing.Amazon.Sqs.Builder
{
    public interface IAwsConfig
    {
        TClient CreateAwsClient<TClient>(ClientConfig clientConfig) where TClient : AmazonServiceClient;
    }
}