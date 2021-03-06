﻿using System;
using System.Collections.Generic;
using System.Linq;
using Esb.Transport;

namespace Esb.Message
{
    public class MessageQueue : IMessageQueue
    {
        private readonly List<Envelope> _list = new List<Envelope>();

        public void Add(Envelope message)
        {
            lock (_list)
            {
                lock (_suspendMessages)
                {
                    var messageType = message.MessageType;
                    if (_suspendMessages.ContainsKey(messageType))
                    {
                        _suspendMessages[messageType].Add(message);
                    }
                    else
                    {
                        _list.Add(message);
                        RaiseMessageArrived();
                    }
                }
            }
        }

        public IEnumerable<Envelope> Messages
        {
            get
            {
                lock (_list)
                {
                    foreach (var envelope in _list.OrderByDescending(o => o.Priority).ThenBy(o => o.CreatedOn))
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
                var result = _list.OrderByDescending(o => o.Priority).FirstOrDefault();
                if (result != null)
                    _list.Remove(result);
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

        private void RaiseMessageArrived()
        {
            var handler = OnMessageArived;
            handler?.Invoke(this, new EventArgs());
        }

        public event EventHandler<EventArgs> OnMessageArived;

        public void RerouteMessages(Type messageType)
        {
            if (Router == null)
                throw new Exception("Cannot reroute any message, because Router is null.");
            lock (_list)
            {
                foreach (var envelope in _list.Where(o => o.MessageType == messageType).ToList())
                {
                    _list.Remove(envelope);
                    Router.Process(envelope);
                }
            }
        }

        public void RemoveMessages(Type messageType)
        {
            lock (_list)
            {
                _list.RemoveAll(o => o.MessageType == messageType);
                _suspendMessages.Remove(messageType);
            }
        }

        public IRouter Router { get; set; }
    }
}