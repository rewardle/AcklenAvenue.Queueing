namespace AcklenAvenue.Queueing.Amazon.Sqs.Builder
{
    public class AwsConfig
    {
        public AwsConfig(string accessKey, string secretKey)
        {
            AccessKey = accessKey;
            SecretKey = secretKey;
        }

        public string AccessKey { get; set; }

        public string SecretKey { get; set; }
    }
}