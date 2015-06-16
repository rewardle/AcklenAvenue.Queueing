using Machine.Specifications;

using AWS = Amazon;

namespace AcklenAvenue.Queueing.Amazon.Sqs.IntegrationSpecs
{
    public class when_sending_a_message_from_sns
    {
        static IMessageSender<FakeMessage> _sender;

        static readonly FakeMessage _fakeMessage = new FakeMessage();

        static ISendResponse _result;

        Establish context = () =>
            {
                string acces = "acces";
                string scrt = "key+";
                string ServiceUrl = "http://sns.us-west-2.amazonaws.com";

                string arnAwsSnsUsWestTest = "arn:aws:sns:us-west-2:487799950875:Test";
                _sender = new AWSSnsSender<FakeMessage>(acces, scrt, ServiceUrl, arnAwsSnsUsWestTest);
            };

        Because of = () => _result = _sender.Send(_fakeMessage);

        It should_recieve_message_in_subscribres = () => _result.ShouldNotBeNull();
    }
}