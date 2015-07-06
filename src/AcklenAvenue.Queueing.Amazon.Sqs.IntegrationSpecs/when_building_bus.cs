using AcklenAvenue.Queueing.Amazon.Sqs.Builder;

using Autofac;

using Machine.Specifications;

namespace AcklenAvenue.Queueing.Amazon.Sqs.Specs.Integration
{
    public class when_building_bus
    {
        static IContainer _container;

        Establish context = () => { };

        Because of = () =>
            {
                var builder = new BusBuilder("clientid ", "secret");
                var containerBuilder = new ContainerBuilder();

                builder.ConfigureSns(
                    "http://sns.us-west-2.amazonaws.com", "arn")
                       .ConfigureSqs("http://sqs.us-west-2.amazonaws.com")
                       .ConfigureCommandQueues<FakeCommand>("fakeQueueCommand")
                       .SetMaxNumberOfMessages(10)
                       .SetVisibilityTimeOut(120)
                       .UseSerializer<TestSerializer>()
                       .UseDeserializer<TestSerializer>()
                       .ConfigureEventQueue<FakeEvent>("fakeQueueEvents")
                       .SetMaxNumberOfMessages(10)
                       .SetVisibilityTimeOut(120)
                       .BuildInContainer(containerBuilder);

                _container = containerBuilder.Build();
            };

        It should_resolve_command_sender = () => _container.Resolve<IMessageSender<FakeCommand>>();
    }

    class FakeEvent
    {
    }

    class FakeCommand
    {
    }
}