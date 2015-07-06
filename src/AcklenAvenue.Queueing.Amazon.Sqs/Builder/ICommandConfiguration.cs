using Autofac;

namespace AcklenAvenue.Queueing.Amazon.Sqs.Builder
{
    public interface ICommandConfiguration : IQueueConfiguration
    {
        void Build(ContainerBuilder containerBuilder, AwsConfig awsConfig, SqsConfig sqsConfiguration);
    }
}