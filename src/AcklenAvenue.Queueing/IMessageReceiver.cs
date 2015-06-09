using System.Collections.Generic;

namespace AcklenAvenue.Queueing
{
    public interface IMessageReceiver<TMessage>
    {
        IEnumerable<IMessageReceived<TMessage>> Receive();
    }
}