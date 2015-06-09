using System.Collections.Generic;
using System.Threading.Tasks;

namespace AcklenAvenue.Queueing
{
    public interface IMessageReceiver<TMessage>
    {
        Task<IEnumerable<IMessageReceived<TMessage>>> ReceiveMessage();
    }
}