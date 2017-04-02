using System;
using System.Threading.Tasks;
using Esb.Cluster;
using Esb.Cluster.Messages;
using Esb.Cluster.Proccessors;
using Esb.Message;
using Esb.Transport;
using AddNodeToCluster = Esb.Cluster.Proccessors.AddNodeToCluster;
using AskForClusterConfiguration = Esb.Cluster.Proccessors.AskForClusterConfiguration;
using ClusterConfiguration = Esb.Cluster.ClusterConfiguration;

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
            InitialStartUpAync();
        }

        public WorkerStatus Status { get; private set; } = WorkerStatus.Stopped;

        public void InitialStartUpAync()
        {
            Task.Factory.StartNew(() =>
            {
                Start();
            });
        }

        private void FindClusterAndEstablishCommunication()
        {
            if (_workerConfiguration.ControllerNodes.Empty() && IsController)
                return;

            foreach (var controllerNode in _workerConfiguration.ControllerNodes)
            {
                if (_router.ProcessSync(new Envelope(new AskForClusterConfiguration(), Priority.Administrative),
                    new NodeConfiguration(this, Guid.Empty, controllerNode)))
                    break;
            }
        }

        private void AddClusterCommunicationProcessors()
        {
            LocalNode.Processors.Add(new RemoveNodeFromCluster());
            LocalNode.Processors.Add(new AddNodeToCluster());
            LocalNode.Processors.Add(new SyncClusterConfiguration());

            if (!IsController)
                return;

            LocalNode.Processors.Add(new AskForClusterConfiguration());
        }

        private void CreateLocalNodeConfiguration()
        {
            LocalNode = new NodeConfiguration(this, _workerConfiguration.NodeId, _workerConfiguration.Address);
        }

        public void Start()
        {
            if (Status != WorkerStatus.Stopped)
                throw new Exception("Cannot start worker, because it is not stopped");
            Status = WorkerStatus.Initialization;
            FindClusterAndEstablishCommunication();
            Status = WorkerStatus.Starting;
            SetLocalNodeOnline();
            SendOnlineMessage();
            Status = WorkerStatus.Started;
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
            if (Status != WorkerStatus.Starting)
                throw new Exception("Cannot stop worker, because it is not started");

            Status = WorkerStatus.Stopping;
            SendOfflineMessage();
            SetLocalNodeOffline();
            Status = WorkerStatus.Stopped;
        }

        private void SetLocalNodeOffline()
        {
            _clusterConfiguration.RemoveNode(LocalNode);
        }

        private void SendOfflineMessage()
        {
            _router.Process(new Envelope(new RemoveNodeFromClusterMessage(LocalNode), Priority.Administrative));
        }

        public bool IsController => _workerConfiguration.IsControllerNode;
    }

    public enum WorkerStatus
    {
        Initialization, Starting, Started, Stopping, Stopped
    }
}
