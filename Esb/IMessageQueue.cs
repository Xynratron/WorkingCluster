using System;
using System.Collections.Generic;

namespace Esb
{
    public interface IMessageQueue
    {
        void Add(Envelope message);
        IEnumerable<Envelope> Messages { get; }
        Envelope GetNextMessage();
        void SuspendMessages(Type messageType);
        void ResumeMessages(Type messageType);
        void RerouteMessages(Type messageType);
        
    }
}