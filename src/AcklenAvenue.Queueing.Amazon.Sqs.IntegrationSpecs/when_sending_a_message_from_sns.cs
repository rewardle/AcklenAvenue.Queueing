using System.Threading.Tasks;
using AcklenAvenue.Queueing.Amazon.Sqs.Builder;
using Machine.Specifications;

namespace AcklenAvenue.Queueing.Amazon.Sqs.Specs.Integration
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
                _sender = new AWSSnsSender<FakeMessage>(new IAMRolesConfig(), ServiceUrl, arnAwsSnsUsWestTest, new TestSerializer());
            };

        Because of = () =>
                     {
                         Task<ISendResponse> sendTask = _sender.Send(_fakeMessage);
                         sendTask.Wait();
                         _result = sendTask.Result;
                     };

        It should_recieve_message_in_subscribres = () => _result.ShouldNotBeNull();
    }
}