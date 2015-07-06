using Autofac;

namespace AcklenAvenue.Queueing.Amazon.Sqs.Builder
{
    public class QueueConfigurationBase : IQueueConfiguration
    {
        public int MaxNumberOfMessages { get; set; }

        public int VisibilityTimeOut { get; set; }

        public BusBuilder Builder { get; set; }

        public IQueueConfiguration SetMaxNumberOfMessages(int max)
        {
            MaxNumberOfMessages = 10;

            return this;
        }

        public BusBuilder SetVisibilityTimeOut(int timeOut)
        {
            VisibilityTimeOut = 120;
            return Builder;
        }

        
    }
}