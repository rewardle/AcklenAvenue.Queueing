using Autofac;

namespace AcklenAvenue.Queueing.Amazon.Sqs.Builder
{
    public interface ICommandConfiguration : IQueueConfiguration
    {
        void Build(ContainerBuilder containerBuilder, IAwsConfig awsConfig, SqsConfig sqsConfiguration, SnsConfig snsConfiguration);
    }
}