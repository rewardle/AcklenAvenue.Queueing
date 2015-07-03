using Autofac;

namespace AcklenAvenue.Queueing.Amazon.Sqs.Builder
{
    public class CommandConfiguration<TCommandQueueMessage> : QueueConfigurationBase, ICommandConfiguration
    {
        public CommandConfiguration(string queueName)
        {
            QueueName = queueName;
        }

        public string QueueName { get; set; }

        public void Build(ContainerBuilder containerBuilder, AwsConfig awsConfig, SqsConfig sqsConfiguration)
        {
            string awsAccessKeyId = awsConfig.AccessKey;
            string awsSecretAccessKey = awsConfig.SecretKey;
            string sqsServiceUrl = sqsConfiguration.SqsServiceUrl;
            string commandQueue = QueueName;
            string commandQueueUrl =
                new QueueCreator(awsAccessKeyId, awsSecretAccessKey, sqsServiceUrl).CreateQueue(commandQueue);

            containerBuilder.Register(
                context =>
                new NormalAWSSqsReceiver<TCommandQueueMessage>(
                    awsAccessKeyId, awsSecretAccessKey, sqsServiceUrl, commandQueueUrl)
                    {
                        MaxNumberOfMessages =
                            MaxNumberOfMessages,
                        VisibilityTimeOut =
                            VisibilityTimeOut
                    })
                            .As<IMessageReceiver<TCommandQueueMessage>>();

            containerBuilder.Register(
                context =>
                new SqsMessageDeleter<TCommandQueueMessage>(
                    awsAccessKeyId, awsSecretAccessKey, sqsServiceUrl, commandQueueUrl))
                            .As<IMessageDeleter<TCommandQueueMessage>>();

            containerBuilder.Register(
                context =>
                new AWSSqsSender<TCommandQueueMessage>(
                    awsAccessKeyId, awsSecretAccessKey, sqsServiceUrl, commandQueueUrl))
                            .As<IMessageSender<TCommandQueueMessage>>();
        }
    }
}