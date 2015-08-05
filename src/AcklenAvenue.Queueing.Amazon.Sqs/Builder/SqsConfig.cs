namespace AcklenAvenue.Queueing.Amazon.Sqs.Builder
{
    public class SqsConfig
    {
        public SqsConfig(string sqsServiceUrl)
        {
            SqsServiceUrl = sqsServiceUrl;
        }

        public string SqsServiceUrl { get; set; }
    }
}