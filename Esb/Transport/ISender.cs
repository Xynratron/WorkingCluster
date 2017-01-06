namespace Esb.Transport
{
    /// <summary>
    /// Used to send a message over the transportation layer
    /// </summary>
    public interface ISender
    {
        void Send(Envelope messageEnvelope);
        void Send(Envelope message, INodeConfiguration node);
    }
}