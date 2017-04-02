using Esb.Message;

namespace Esb.Transport
{
    public interface IRouter
    {
        void Process(Envelope message);
    }
}