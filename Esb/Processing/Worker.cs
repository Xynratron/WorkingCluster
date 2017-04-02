using System;
using Esb.Cluster;
using Esb.Cluster.Proccessors;
using Esb.Message;
using Esb.Transport;

namespace Esb.Processing
{
    /// <summary>
    /// The local worker process; has a own node with configuration
    /// </summary>
    public class Worker : IWorker
    {
        public INodeConfiguration LocalNode { get; private set; }
        
        private IMessageQueue _messageQueue;
        private readonly IRouter _router;

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
           //_workerConfiguration.RootNodes
        }

        private void AddClusterCommunicationProcessors()
        {
            LocalNode.Processors.Add(new RemoveNodeFromCluster());
            LocalNode.Processors.Add(new AddNodeToCluster());
        }

        private void CreateLocalNodeConfiguration()
        {
            LocalNode = new NodeConfiguration(this, _workerConfiguration.NodeId, _workerConfiguration.Address);
        }

        public void Start()
        {
            SetLocalNodeOnline();
            SendOnlineMessage();
        }

        private void SetLocalNodeOnline()
        {
            _clusterConfiguration.AddNode(LocalNode);
        }

        private void SendOnlineMessage()
        {
            _router.Process(new Envelope(new Cluster.Messages.AddNodeToCluster(LocalNode), Priority.Administrative));
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
            _router.Process(new Envelope(new Cluster.Messages.RemoveNodeFromCluster(LocalNode), Priority.Administrative));
        }

        public bool IsController => _workerConfiguration.IsControllerNode;
    }
}
