using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Esb
{
    /// <summary>
    /// The local worker process; has a own node with configuration
    /// </summary>
    public class Worker
    {
        public INodeConfiguration LocalNode { get; set; }

        private IMessageQueue _messageQueue;

        public Worker(IMessageQueue messageQueue)
        {
            _messageQueue = messageQueue;
        }

        public void Start()
        {
            
        }

        public void Stop()
        {

        }
    }
}
