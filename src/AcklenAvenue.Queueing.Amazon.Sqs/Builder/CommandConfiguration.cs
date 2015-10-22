using System;

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

        public void Build(ContainerBuilder containerBuilder, IAwsConfig awsConfig, SqsConfig sqsConfiguration, SnsConfig snsConfiguration)
        {
           
            string sqsServiceUrl = sqsConfiguration.SqsServiceUrl;
            string snsServiceUrl = snsConfiguration.SnsServiceUrl;
            string commandQueue = QueueName;
            string topicArn = snsConfiguration.TopicArn;

            string commandQueueUrl =
                new QueueCreator(awsConfig, sqsServiceUrl).CreateQueue(commandQueue);

            containerBuilder.Register(
                context =>
                new NormalAWSSqsReceiver<TCommandQueueMessage>(
                    awsConfig, sqsServiceUrl, commandQueueUrl, ResolverDeserialzer(context))
                    {
                        MaxNumberOfMessages
                            =
                            MaxNumberOfMessages,
                        VisibilityTimeOut
                            =
                            VisibilityTimeOut
                    })
                            .As<IMessageReceiver<TCommandQueueMessage>>();

            containerBuilder.Register(
                context =>
                new SqsMessageDeleter<TCommandQueueMessage>(
                    awsConfig, sqsServiceUrl, commandQueueUrl))
                            .As<IMessageDeleter<TCommandQueueMessage>>();

            containerBuilder.Register(
                context =>
                new AWSSnsSender<TCommandQueueMessage>(
                    awsConfig, snsServiceUrl, topicArn, ResoverSerialzer(context)))
                            .As<IMessageSender<TCommandQueueMessage>>();

            CommandQueueSubscribesToNotifications(
                awsConfig, snsServiceUrl, sqsServiceUrl, commandQueueUrl, topicArn);
        }

        static IMessageSerializer ResoverSerialzer(IComponentContext context)
        {
            if (!context.IsRegistered<IMessageSerializer>())
            {
                throw new Exception(
                    string.Format(
                        "There is no message Serializer in the container, please register an implementation for IMessageSerializer in the container"));
            }
            var serializer = context.Resolve<IMessageSerializer>();
            return serializer;
        }

        static IMessageDeserializer ResolverDeserialzer(IComponentContext context)
        {
            if (!context.IsRegistered<IMessageDeserializer>())
            {
                throw new Exception(
                    string.Format(
                        "There is no message deserializer in the container, please register an implementation for IMessageDeserializer in the container"));
            }
            var serializer = context.Resolve<IMessageDeserializer>();
            return serializer;
        }

        static void CommandQueueSubscribesToNotifications(
            IAwsConfig awsConfig,
            string snsServiceUrl,
            string sqsServiceUrl,
            string commandQueueUrl,
            string topicArn)
        {
            var notificationSubscriber = new AWSSnsSubcriber(
                awsConfig, snsServiceUrl, sqsServiceUrl, topicArn);
            notificationSubscriber.Subscribe(commandQueueUrl);
        }
    }
}