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
                var builder = new BusBuilder("AKIAICDZWES32E4QZQNA", "UrEIS7fz0qb1MqJnei0mHOTfHjiwU0XRg7rhX8S+");
                var containerBuilder = new ContainerBuilder();

                builder.ConfigureSns(
                    "http://sns.us-west-2.amazonaws.com", "arn:aws:sns:us-west-2:487799950875:Rewardle_local_saul")
                       .ConfigureSqs("http://sqs.us-west-2.amazonaws.com")
                       .ConfigureCommandQueues<FakeCommand>("fakeQueueCommand")
                       .SetMaxNumberOfMessages(10)
                       .SetVisibilityTimeOut(120)
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