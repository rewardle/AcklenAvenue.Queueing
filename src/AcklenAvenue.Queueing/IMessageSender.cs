using System.Threading.Tasks;

namespace AcklenAvenue.Queueing
{
    public interface IMessageSender<in TMessage>
    {
        ISendResponse Send(TMessage message);
    }
}