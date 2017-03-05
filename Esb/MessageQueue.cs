using System;
using System.Collections.Generic;
using System.Linq;

namespace Esb
{
    public class MessageQueue : IMessageQueue
    {
        List<Envelope> _list = new List<Envelope>();
        public void Add(Envelope message)
        {
            lock (_list)
            {
                _list.Add(message);
            }
        }

        public IEnumerable<Envelope> Messages
        {
            get
            {
                lock (_list)
                {
                    foreach (var envelope in _list)
                    {
                        yield return envelope;
                    }
                }
            }
        }
        public Envelope GetNextMessage()
        {
            lock (_list)
            {
                var result = _list.FirstOrDefault();
                if (result != null)
                    _list.RemoveAt(0);
                return result;
            }
        }

        private readonly Dictionary<Type, List<Envelope>> _suspendMessages = new Dictionary<Type, List<Envelope>>();

        public void SuspendMessages(Type messageType)
        {
            lock (_list)
            {
                lock (_suspendMessages)
                {
                    _suspendMessages.Add(messageType, _list.Where(o => o.MessageType == messageType).ToList());
                    _list.RemoveAll(o => o.MessageType == messageType);
                }
            }
        }

        public void ResumeMessages(Type messageType)
        {
            lock (_list)
            {
                lock (_suspendMessages)
                {
                    _list.AddRange(_suspendMessages[messageType]);
                    _suspendMessages.Remove(messageType);
                }
            }
        }

        public void RerouteMessages(Type messageType)
        {
            throw new NotImplementedException();
        }
    }
}