using System;
using Amazon.Runtime;

namespace AcklenAvenue.Queueing.Amazon.Sqs.Builder
{
    public class KeyAwsConfig
        : IAwsConfig
    {
        public KeyAwsConfig(string accessKey, string secretKey)
        {
            AccessKey = accessKey;
            SecretKey = secretKey;
        }

        public string AccessKey { get; set; }
        public string SecretKey { get; set; }

        public TClient CreateAwsClient<TClient>(ClientConfig clientConfig) where TClient : AmazonServiceClient
        {
            var instance = Activator.CreateInstance(typeof (TClient), AccessKey, SecretKey, clientConfig);


            return (TClient) instance;
        }
    }
}