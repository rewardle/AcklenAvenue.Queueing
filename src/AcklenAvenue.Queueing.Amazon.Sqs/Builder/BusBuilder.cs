using System;

using Autofac;

namespace AcklenAvenue.Queueing.Amazon.Sqs.Builder
{
    public class BusBuilder
    {
        public BusBuilder(string accessKey, string secretKey)
        {
            KeyAwsConfig = new KeyAwsConfig(accessKey, secretKey);
            SnsConfiguration = new SnsConfig(string.Empty, string.Empty);
            SqsConfiguration = new SqsConfig(string.Empty);
        }

        protected SnsConfig SnsConfiguration { get; set; }

        protected SqsConfig SqsConfiguration { get; set; }

        protected ICommandConfiguration CommandQueueConfiguration { get; set; }

        protected IEventConfiguration EventQueueConfiguration { get; set; }

        public IAwsConfig KeyAwsConfig { get; private set; }

        protected Type SerializerType { get; set; }

        protected Type DeserializerType { get; set; }

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

        public BusBuilder UseSerializer<TSerialiezer>() where TSerialiezer : IMessageSerializer
        {
            SerializerType = typeof(TSerialiezer);
            return this;
        }

        public BusBuilder UseDeserializer<TDeserializer>() where TDeserializer : IMessageDeserializer
        {
            DeserializerType = typeof(TDeserializer);
            return this;
        }

        public void BuildInContainer(ContainerBuilder builder)
        {
            if (SerializerType != null)
            {
                builder.RegisterType(SerializerType).As<IMessageSerializer>();
            }
            else
            {
                throw new Exception(
                    string.Format(
                        "No serializer is registered to build the queue, please call the method 'UseSerializer<>()' to set a serialzer"));
            }

            if (DeserializerType != null)
            {
                builder.RegisterType(DeserializerType).As<IMessageDeserializer>();
            }
            else
            {
                throw new Exception(
                    string.Format(
                        "No deserializer is registered to build the queue, please call the method 'UseDeserializer<>()' to set a deserialzer"));
            }

            CommandQueueConfiguration.Build(builder, KeyAwsConfig, SqsConfiguration);
            EventQueueConfiguration.Build(builder, KeyAwsConfig, SqsConfiguration, SnsConfiguration);
        }
    }
}