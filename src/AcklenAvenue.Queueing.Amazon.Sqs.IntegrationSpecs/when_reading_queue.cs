using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcklenAvenue.Queueing.Amazon.Sqs.Builder;
using Machine.Specifications;

namespace AcklenAvenue.Queueing.Amazon.Sqs.Specs.Integration
{
    public class when_reading_queue : given_sqs_client_context
    {
        static IMessageReceiver<FakeMessage> _messageReceiver;

        static IEnumerable<IMessageReceived<FakeMessage>> _result;

        static FakeMessage _fakeMessage;

        static ISendResponse _response;

        Establish context = () =>
            {
                var sender = new AWSSqsSender<FakeMessage>(
                    new IAMRolesConfig(), ServiceUrl, CreateQueueResponse.QueueUrl, new TestSerializer());
                _fakeMessage = new FakeMessage { Greet = "hi" };
                Task<ISendResponse> sendTask = sender.Send(_fakeMessage);
                sendTask.Wait();
                _response = sendTask.Result;

                _messageReceiver = new NormalAWSSqsReceiver<FakeMessage>(
                    new IAMRolesConfig(), ServiceUrl, CreateQueueResponse.QueueUrl, new TestSerializer());
            };

        Because of = () =>
                     {
                         Task<IEnumerable<IMessageReceived<FakeMessage>>> receiveTask = _messageReceiver.Receive();
                         receiveTask.Wait();
                         _result = receiveTask.Result;
                     };

        It should_return_message_of_the_same_type = () => _result.First().Message.ShouldBeAssignableTo<FakeMessage>();

        It should_return_the_list_of_messages = () => _result.Count().ShouldEqual(1);
    }
}