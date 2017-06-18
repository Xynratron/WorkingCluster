using System.Linq;
using Esb.Cluster;
using Esb.Message;

namespace Esb.Processing
{
    public class SyncMessageWorkFactory
    {
        private IMessageQueue _messageQueue;
        private INodeConfiguration _node;
        private IEnvironment _environment;

        public SyncMessageWorkFactory(IMessageQueue messageQueue, INodeConfiguration node, IEnvironment environment)
        {
            _messageQueue = messageQueue;
            _node = node;
            _environment = environment;
            _messageQueue.OnMessageArived += (sender, e) => StartWithMessageProcessing();
        }

        public bool MustCancelWork = false;

        private volatile bool inFetching = false;
        private object syncLock = new object();

        public void StartWithMessageProcessing()
        {
            try
            {
                lock (syncLock)
                {
                    if (inFetching)
                        return;

                    inFetching = true;
                }
                var message = _messageQueue.GetNextMessage();
                while (message != null)
                {
                    var processors = _node.Processors.Where(o => o.ProcessingType == message.MessageType).ToList();
                    foreach (var processor in processors)
                    {
                        var o = processor.GetInstance;
                        var method = processor.GetType().GetMethod("Process");
                        method.Invoke(o, new object[] {_environment, message, message.Message});
                    }
                    message = _messageQueue.GetNextMessage();
                    if (MustCancelWork)
                        return;
                }
            }
            finally
            {
                lock (syncLock)
                {
                    inFetching = false;
                }
            }
        }
    }
}