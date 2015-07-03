using System.Collections.Generic;
using System.Linq;

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
                    acces, scrt, ServiceUrl, CreateQueueResponse.QueueUrl, new TestSerializer());
                _fakeMessage = new FakeMessage { Greet = "hi" };
                _response = sender.Send(_fakeMessage);

                _messageReceiver = new NormalAWSSqsReceiver<FakeMessage>(
                    acces, scrt, ServiceUrl, CreateQueueResponse.QueueUrl, new TestSerializer());
            };

        Because of = () => { _result = _messageReceiver.Receive(); };

        It should_return_message_of_the_same_type = () => _result.First().Message.ShouldBeAssignableTo<FakeMessage>();

        It should_return_the_list_of_messages = () => _result.Count().ShouldEqual(1);
    }
}