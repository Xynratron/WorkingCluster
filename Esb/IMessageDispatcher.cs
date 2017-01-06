using Esb.Transport;

namespace Esb
{
    public interface IMessageDispatcher
    {
        bool IsUnderPressure();
    }
}