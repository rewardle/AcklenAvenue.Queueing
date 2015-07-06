namespace AcklenAvenue.Queueing.Amazon.Sqs.Builder
{
    public class SnsConfig
    {
        public SnsConfig(string snsServiceUrl, string topicArn)
        {
            SnsServiceUrl = snsServiceUrl;
            TopicArn = topicArn;
        }

        public string SnsServiceUrl { get; set; }

        public string TopicArn { get; set; }
    }
}