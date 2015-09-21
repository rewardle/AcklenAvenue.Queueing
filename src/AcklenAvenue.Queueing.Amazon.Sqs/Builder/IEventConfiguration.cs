using Autofac;

namespace AcklenAvenue.Queueing.Amazon.Sqs.Builder
{
    public interface IEventConfiguration : IQueueConfiguration
    {
        void Build(ContainerBuilder containerBuilder, IAwsConfig awsConfig, SqsConfig sqsConfiguration, SnsConfig snsConfiguration);
    }
}