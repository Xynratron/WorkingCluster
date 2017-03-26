using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Esb
{
    public class WorkerConfiguration
    {
        public bool IsRootNode { get; set; }
        public Guid NodeId { get; set; }
        public Uri Address { get; set; }
        public List<Uri> RootNodes { get; set; }
    }

    public interface IWorker
    {
        INodeConfiguration LocalNode { get; }
    }

    /// <summary>
    /// The local worker process; has a own node with configuration
    /// </summary>
    public class Worker : IWorker
    {
        public INodeConfiguration LocalNode { get; private set; }

        private IMessageQueue _messageQueue;

        private WorkerConfiguration _workerConfiguration;

        public Worker(WorkerConfiguration workerConfiguration, IMessageQueue messageQueue)
        {
            _workerConfiguration = workerConfiguration;
            _messageQueue = messageQueue;

            CreateLocalNodeConfiguration();
            AddClusterCommunicationProcessors();
            FindClusterAndEstablishCommunication();
        }

        private void AddClusterCommunicationProcessors()
        {
            LocalNode.Processors.Add();
        }

        private void CreateLocalNodeConfiguration()
        {
            LocalNode = new NodeConfiguration(this, _workerConfiguration.NodeId, _workerConfiguration.Address);
        }

        public void Start()
        {
            SendOnlineMessage();
            SetLocalNodeOnline();
        }

        public void Stop()
        {
            SendOfflineMessage();
            SetLocalNodeOffline();
        }
    }

    
}
