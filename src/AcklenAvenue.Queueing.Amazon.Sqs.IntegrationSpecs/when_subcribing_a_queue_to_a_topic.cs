using Machine.Specifications;

namespace AcklenAvenue.Queueing.Amazon.Sqs.IntegrationSpecs
{
    public class when_subcribing_a_queue_to_a_topic
    {
        static IQueueSubcriber _queueSubcriber;

        static string _queueUrl;

        static string _topicArn;

        static ISubscribeReponseMessage _result;

        Establish context = () =>
            {
                string acces = "acces";
                string scrt = "key+";
                const string SnsServiceUrl = "http://sns.us-west-2.amazonaws.com";
                const string sqsServiceUrl = "http://sqs.us-west-2.amazonaws.com";
                _queueUrl = "https://sqs.us-west-2.amazonaws.com/487799950875/otra";
                _topicArn = "arn:aws:sns:us-west-2:487799950875:Test";

                _queueSubcriber = new AWSSnsSubcriber(acces, scrt, SnsServiceUrl, sqsServiceUrl, _topicArn);
            };

        Because of = () => _result = _queueSubcriber.Subscribe(_queueUrl);

        It should_have_the_subcription_in_the_topic = () => _result.ShouldNotBeNull();
    }
}