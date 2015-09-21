using System;
using Amazon.Runtime;

namespace AcklenAvenue.Queueing.Amazon.Sqs.Builder
{
    public class IAMRolesConfig :
        IAwsConfig
    {
        public TClient CreateAwsClient<TClient>(ClientConfig clientConfig) where TClient : AmazonServiceClient
        {
            var instance = Activator.CreateInstance(typeof (TClient), clientConfig);

            return (TClient) instance;
        }
    }
}