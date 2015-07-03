using Autofac;

namespace AcklenAvenue.Queueing.Amazon.Sqs.Builder
{
    public class BusBuilder
    {
        public BusBuilder(string accessKey, string secretKey)
        {
            AwsConfig = new AwsConfig(accessKey, secretKey);
            SnsConfiguration = new SnsConfig(string.Empty, string.Empty);
            SqsConfiguration = new SqsConfig(string.Empty);
        }

        protected SnsConfig SnsConfiguration { get; set; }

        protected SqsConfig SqsConfiguration { get; set; }

        protected ICommandConfiguration CommandQueueConfiguration { get; set; }

        protected IEventConfiguration EventQueueConfiguration { get; set; }

        public AwsConfig AwsConfig { get; private set; }

        public BusBuilder ConfigureSns(string snsServiceUrl, string topicArn)
        {
            SnsConfiguration = new SnsConfig(snsServiceUrl, topicArn);

            return this;
        }

        public BusBuilder ConfigureSqs(string sqsServiceUrl)
        {
            SqsConfiguration = new SqsConfig(sqsServiceUrl);
            return this;
        }

        public ICommandConfiguration ConfigureCommandQueues<TCommandMessage>(string queueName)
        {
            CommandQueueConfiguration = new CommandConfiguration<TCommandMessage>(queueName) { Builder = this };
            return CommandQueueConfiguration;
        }

        public IEventConfiguration ConfigureEventQueue<TEventMessage>(string queueName)
        {
            EventQueueConfiguration = new EventConfiguration<TEventMessage>(queueName) { Builder = this };
            return EventQueueConfiguration;
        }

        public void BuildInContainer(ContainerBuilder builder)
        {
            CommandQueueConfiguration.Build(builder, AwsConfig, SqsConfiguration);
            EventQueueConfiguration.Build(builder,AwsConfig,SqsConfiguration, SnsConfiguration);
        }
    }
}