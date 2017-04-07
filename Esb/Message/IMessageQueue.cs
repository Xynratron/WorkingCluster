using System;
using System.Collections.Generic;
using Esb.Transport;

namespace Esb.Message
{
    public interface IMessageQueue
    {
        void Add(Envelope message);
        IEnumerable<Envelope> Messages { get; }
        Envelope GetNextMessage();
        void SuspendMessages(Type messageType);
        void ResumeMessages(Type messageType);
        void RerouteMessages(Type messageType);
        void RemoveMessages(Type messageType);

        event EventHandler<EventArgs> OnMessageArived;

        IRouter Router { get; set; }
    }
}