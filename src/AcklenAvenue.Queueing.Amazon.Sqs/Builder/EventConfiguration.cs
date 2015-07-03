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

        public void Build(
            ContainerBuilder containerBuilder,
            AwsConfig awsConfig,
            SqsConfig sqsConfiguration,
            SnsConfig snsConfiguration)
        {
            string awsAccessKeyId = awsConfig.AccessKey;
            string awsSecretAccessKey = awsConfig.SecretKey;
            string sqsServiceUrl = sqsConfiguration.SqsServiceUrl;
            string snsServiceUrl = snsConfiguration.SnsServiceUrl;
            string eventQueue = QueueName;
            string topicArn = snsConfiguration.TopicArn;

            string eventQueueUrl =
                new QueueCreator(awsAccessKeyId, awsSecretAccessKey, sqsServiceUrl).CreateQueue(eventQueue);

            containerBuilder.Register(
                context =>
                new SnsMessageSqsReceiver<TEventMessage>(
                    awsAccessKeyId, awsSecretAccessKey, sqsServiceUrl, eventQueueUrl)
                    {
                        MaxNumberOfMessages = MaxNumberOfMessages,
                        VisibilityTimeOut = VisibilityTimeOut
                    })
                            .As<IMessageReceiver<TEventMessage>>();

            containerBuilder.Register(
                context => new AWSSnsSender<TEventMessage>(awsAccessKeyId, awsSecretAccessKey, snsServiceUrl, topicArn))
                            .As<IMessageSender<TEventMessage>>();

            containerBuilder.Register(
                context =>
                new SqsMessageDeleter<TEventMessage>(awsAccessKeyId, awsSecretAccessKey, sqsServiceUrl, eventQueueUrl))
                            .As<IMessageDeleter<TEventMessage>>();

            EventQueueSubscribesToNotifications(
                awsAccessKeyId, awsSecretAccessKey, snsServiceUrl, sqsServiceUrl, eventQueueUrl, topicArn);
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