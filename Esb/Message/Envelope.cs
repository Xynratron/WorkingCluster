using System;
using System.Collections.Generic;

namespace Esb.Message
{
    /// <summary>
    /// an envelope encapsules a single message which can then be sent over the transportation layer
    /// </summary>
    public class Envelope
    {
        public Envelope(object message, Priority priority, Guid? transactionId = null)
        {
            CreatedOn = DateTime.Now;
            Id = Guid.NewGuid();
            TransactionId = transactionId??Guid.Empty;
            Message = message;
            MessageType = message.GetType();
            Headers = new List<string>();
            Priority = priority;
        }

        public object Message { get; }
        public Type MessageType { get; }
        public DateTime CreatedOn { get; }
        public Guid Id { get; }
        public Guid TransactionId { get; }
        public List<string> Headers { get; }

        public void AddHeaderLine(string headerLine)
        {
            Headers.Add(headerLine);
        }

        public Priority Priority { get; }
    }
}