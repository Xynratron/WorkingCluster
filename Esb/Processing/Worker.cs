using System;
using System.Linq;
using System.Threading.Tasks;
using Esb.Cluster;
using Esb.Cluster.Messages;
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
        public WorkerStatus Status { get; private set; } = WorkerStatus.Stopped;

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

        public void AddProcessor(IProcessor processor)
        {
            _clusterConfiguration.AddProcessorsToNode(LocalNode, processor);
            _router.Process(new Envelope(new AddProcessorToNode(LocalNode, processor), Priority.Administrative));
        }

        public void RemoveProcessor(IProcessor processor)
        {
            _messageQueue.SuspendMessages(processor.ProcessingType);
            _clusterConfiguration.RemoveProcessorsFromNode(LocalNode, processor);
            _router.Process(new Envelope(new RemoveProcessorFromNode(LocalNode, processor), Priority.Administrative));
            if (LocalNode.Processors.All(o => o.ProcessingType != processor.ProcessingType))
                _messageQueue.RerouteMessages(processor.ProcessingType);
            else
                _messageQueue.ResumeMessages(processor.ProcessingType);
        }

        private void InitialStartUpAync()
        {
            Task.Factory.StartNew(Start);
        }

        private void FindClusterAndEstablishCommunication()
        {
            if (_workerConfiguration.ControllerNodes.Empty() && IsController)
                return;

            foreach (var controllerNode in _workerConfiguration.ControllerNodes)
            {
                if (_router.ProcessSync(new Envelope(new AskForClusterConfiguration(), Priority.Administrative),
                    new NodeConfiguration(this, controllerNode)))
                    break;
            }
        }

        private void AddClusterCommunicationProcessors()
        {
            LocalNode.Processors.Add(new RemoveNodeFromClusterProcessor());
            LocalNode.Processors.Add(new AddNodeToClusterProcessor());
            LocalNode.Processors.Add(new SyncClusterConfigurationProcessor());

            if (!IsController)
                return;

            LocalNode.Processors.Add(new AskForClusterConfigurationProcessor());
        }

        private void CreateLocalNodeConfiguration()
        {
            LocalNode = new NodeConfiguration(this, _workerConfiguration.Address);
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
            if (Status != WorkerStatus.Started)
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


        public class SyncMessageWorkFactory
        {
            private IMessageQueue _messageQueue;
            public SyncMessageWorkFactory(IMessageQueue messageQueue)
            {
                _messageQueue = messageQueue;
            }
            public bool MustCancelWork = false;

            private bool inFetching = false;

            public void StartWithMessageProcessing()
            {
                inFetching = true;
                var message = _messageQueue.GetNextMessage();
                while (message != null)
                {

                    message = _messageQueue.GetNextMessage();
                }
                inFetching = false;
            }
        }
    }

    
}
