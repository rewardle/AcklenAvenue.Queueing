using System.Threading.Tasks;

namespace AcklenAvenue.Queueing
{
    public interface IMessageSender<in TMessage>
    {
        Task<ISendResponse> SendMessage(TMessage message);
    }
}