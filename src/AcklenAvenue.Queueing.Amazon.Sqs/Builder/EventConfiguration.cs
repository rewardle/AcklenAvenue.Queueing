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

        public void Build(ContainerBuilder containerBuilder, IAwsConfig awsConfig, SqsConfig sqsConfiguration, SnsConfig snsConfiguration)
        {
            string sqsServiceUrl = sqsConfiguration.SqsServiceUrl;
            string snsServiceUrl = snsConfiguration.SnsServiceUrl;
            string eventQueue = QueueName;
            string topicArn = snsConfiguration.TopicArn;

            string eventQueueUrl =
                new QueueCreator(awsConfig, sqsServiceUrl).CreateQueue(eventQueue);

            containerBuilder.Register(
                context =>
                    {
                        IMessageDeserializer serializer = ResolverDeserialzer(context);
                        return new SnsMessageSqsReceiver<TEventMessage>(
                            awsConfig, sqsServiceUrl, eventQueueUrl, serializer)
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
                            awsConfig, snsServiceUrl, topicArn, messageSerializer);
                    }).As<IMessageSender<TEventMessage>>();

            containerBuilder.Register(
                context =>
                new SqsMessageDeleter<TEventMessage>(awsConfig, sqsServiceUrl, eventQueueUrl))
                            .As<IMessageDeleter<TEventMessage>>();

            EventQueueSubscribesToNotifications(
                awsConfig, snsServiceUrl, sqsServiceUrl, eventQueueUrl, topicArn);
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
            IAwsConfig awsConfig,
            string snsServiceUrl,
            string sqsServiceUrl,
            string eventQueueUrl,
            string topicArn)
        {
            var notificationSubscriber = new AWSSnsSubcriber(
                awsConfig, snsServiceUrl, sqsServiceUrl, topicArn);
            notificationSubscriber.Subscribe(eventQueueUrl);
        }
    }
}