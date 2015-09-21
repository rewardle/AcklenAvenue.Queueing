using System;

using Autofac;

namespace AcklenAvenue.Queueing.Amazon.Sqs.Builder
{
    public class EventConfiguration<TEventMessage> : QueueConfigurationBase, IEventConfiguration
    {
        public EventConfiguration(string queueName)
        {
            QueueName = queueName;
        }

        public string QueueName { get; set; }

        public void Build(ContainerBuilder containerBuilder, IAwsConfig keyAwsConfig, SqsConfig sqsConfiguration, SnsConfig snsConfiguration)
        {
            string awsAccessKeyId = keyAwsConfig.AccessKey;
            string awsSecretAccessKey = keyAwsConfig.SecretKey;
            string sqsServiceUrl = sqsConfiguration.SqsServiceUrl;
            string snsServiceUrl = snsConfiguration.SnsServiceUrl;
            string eventQueue = QueueName;
            string topicArn = snsConfiguration.TopicArn;

            string eventQueueUrl =
                new QueueCreator(awsAccessKeyId, awsSecretAccessKey, sqsServiceUrl).CreateQueue(eventQueue);

            containerBuilder.Register(
                context =>
                    {
                        IMessageDeserializer serializer = ResolverDeserialzer(context);
                        return new SnsMessageSqsReceiver<TEventMessage>(
                            awsAccessKeyId, awsSecretAccessKey, sqsServiceUrl, eventQueueUrl, serializer)
                                   {
                                       MaxNumberOfMessages
                                           =
                                           MaxNumberOfMessages,
                                       VisibilityTimeOut
                                           =
                                           VisibilityTimeOut
                                   };
                    }).As<IMessageReceiver<TEventMessage>>();

            containerBuilder.Register(
                context =>
                    {
                        IMessageSerializer messageSerializer = ResoverSerialzer(context);
                        return new AWSSnsSender<TEventMessage>(
                            awsAccessKeyId, awsSecretAccessKey, snsServiceUrl, topicArn, messageSerializer);
                    }).As<IMessageSender<TEventMessage>>();

            containerBuilder.Register(
                context =>
                new SqsMessageDeleter<TEventMessage>(awsAccessKeyId, awsSecretAccessKey, sqsServiceUrl, eventQueueUrl))
                            .As<IMessageDeleter<TEventMessage>>();

            EventQueueSubscribesToNotifications(
                awsAccessKeyId, awsSecretAccessKey, snsServiceUrl, sqsServiceUrl, eventQueueUrl, topicArn);
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

        static void EventQueueSubscribesToNotifications(
            string awsAccessKeyId,
            string awsSecretAccessKey,
            string snsServiceUrl,
            string sqsServiceUrl,
            string eventQueueUrl,
            string topicArn)
        {
            var notificationSubscriber = new AWSSnsSubcriber(
                awsAccessKeyId, awsSecretAccessKey, snsServiceUrl, sqsServiceUrl, topicArn);
            notificationSubscriber.Subscribe(eventQueueUrl);
        }
    }
}