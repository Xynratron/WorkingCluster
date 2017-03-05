using System;

namespace Esb.Transport
{
    /// <summary>
    /// Used to receive a message over the transportation layer
    /// </summary>
    public interface IReceiver
    {
        void Receive(Envelope messageEnvelope);
        Action<Envelope> MessageArrived(Envelope envelope);
    }
}