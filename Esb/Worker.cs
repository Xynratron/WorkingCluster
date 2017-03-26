using System;
using System.Linq;
using System.Text;
using System.Threading;

namespace Esb
{
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

        private void FindClusterAndEstablishCommunication()
        {
            throw new NotImplementedException();
        }

        private void AddClusterCommunicationProcessors()
        {
            throw new NotImplementedException();
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

        private void SetLocalNodeOnline()
        {
            throw new NotImplementedException();
        }

        private void SendOnlineMessage()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            SendOfflineMessage();
            SetLocalNodeOffline();
        }

        private void SetLocalNodeOffline()
        {
            throw new NotImplementedException();
        }

        private void SendOfflineMessage()
        {
            throw new NotImplementedException();
        }
    }

    
}
