using System;
using System.Linq;
using System.Text;
using System.Threading;
using Esb.ClusterCommunication.Messages;
using Esb.Transport;

namespace Esb
{
    /// <summary>
    /// The local worker process; has a own node with configuration
    /// </summary>
    public class Worker : IWorker
    {
        public INodeConfiguration LocalNode { get; private set; }


        private IMessageQueue _messageQueue;
        private IRouter _router;

        private readonly IClusterConfiguration _clusterConfiguration;
        private readonly WorkerConfiguration _workerConfiguration;

        public Worker(WorkerConfiguration workerConfiguration, IRouter router, IMessageQueue messageQueue)
        {
            _workerConfiguration = workerConfiguration;
            _router = router;
            _messageQueue = messageQueue;
            _clusterConfiguration = new ClusterConfiguration();
            
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
            _clusterConfiguration.AddNode(LocalNode);
        }

        private void SendOnlineMessage()
        {
            _router.Process(new Envelope(new AddNodeToCluster(LocalNode), Priority.Administrative));
        }

        public void Stop()
        {
            SendOfflineMessage();
            SetLocalNodeOffline();
        }

        private void SetLocalNodeOffline()
        {
            _clusterConfiguration.RemoveNode(LocalNode);
        }

        private void SendOfflineMessage()
        {
            _router.Process(new Envelope(new RemoveNodeFromCluster(LocalNode), Priority.Administrative));
        }
    }

    
}
