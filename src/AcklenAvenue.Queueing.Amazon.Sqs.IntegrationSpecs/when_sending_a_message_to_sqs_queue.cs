using System.Threading.Tasks;

using Amazon.SQS.Model;

using Machine.Specifications;

using Newtonsoft.Json;

namespace AcklenAvenue.Queueing.Amazon.Sqs.IntegrationSpecs
{
    public class when_sending_a_message_to_sqs_queue : given_sqs_client_context
    {
        static AWSSqsSender<FakeMessage> _messageSender;

        static ISendResponse _result;

        static FakeMessage _fakeMessage;

        Establish context =
            () =>
                {
                    _messageSender = new AWSSqsSender<FakeMessage>(
                        acces, scrt, ServiceUrl, CreateQueueResponse.QueueUrl);
                };

        Because of = () =>
            {
                _fakeMessage = new FakeMessage { Greet = "hi" };
                _messageSender.Delay = 1;
                _result = _messageSender.Send(_fakeMessage);

              
            };

        It should_be_able_to_get_the_message_from_the_queue = () =>
            {
                var receiveMessageRequest = new ReceiveMessageRequest(CreateQueueResponse.QueueUrl);
                receiveMessageRequest.MessageAttributeNames.Add("All");

                ReceiveMessageResponse messsageSended =
                    Sqs.ReceiveMessage(receiveMessageRequest);
                messsageSended.Messages.ShouldNotBeEmpty();
                messsageSended.Messages[0].Body.ShouldEqual(JsonConvert.SerializeObject(_fakeMessage));
                messsageSended.Messages[0].MessageAttributes["Type"].StringValue.ShouldEqual(typeof(FakeMessage).FullName);
            };

        It should_resut_should_not_be_null = () => _result.ShouldNotBeNull();
    }
}