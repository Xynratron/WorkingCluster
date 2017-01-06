namespace Esb.Transport
{
    public interface IRouter
    {
        void Process(Envelope message);
        IReceiver Receiver { get; }
        IMessageQueue MessageQueue { get; }
    }
}