using System;
using Amazon.Runtime;

namespace AcklenAvenue.Queueing.Amazon.Sqs.Builder
{
    public class IAMRolesConfig :
        IAwsConfig
    {
        public TClient CreateClientRequest<TClient>(ClientConfig amazonSqsConfig) where TClient : AmazonServiceClient
        {
            var serviceType = typeof (TClient);


            var ctor = serviceType.GetConstructor(new[] {typeof (ClientConfig)});
            if (ctor == null)
                throw new Exception(
                    string.Format(
                        "Cant create an instance of {0} service, because we cant find the correct constructor",
                        typeof (TClient).Name));



            var instance = ctor.Invoke(new object[] {amazonSqsConfig});

            return (TClient) instance;
        }
    }
}