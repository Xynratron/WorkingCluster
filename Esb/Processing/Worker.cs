using System;
using System.Linq;
using System.Reflection;
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
        public IRouter Router { get; set; }
        public bool IsController => _workerConfiguration.IsControllerNode;
        public IMessageQueue MessageQueue { get; set; }
        public IClusterConfiguration ClusterConfiguration { get; set; }

        private readonly WorkerConfiguration _workerConfiguration;

        private SyncMessageWorkFactory _workingFactory;

        public WorkerStatus Status { get; private set; } = WorkerStatus.Stopped;

        public Worker(WorkerConfiguration workerConfiguration, IClusterConfiguration clusterConfiguration, IRouter router, IMessageQueue messageQueue)
        {
            _workerConfiguration = workerConfiguration;
            Router = router;
            MessageQueue = messageQueue;
            ClusterConfiguration = clusterConfiguration;

            CreateLocalNodeConfiguration();
            AddClusterCommunicationProcessors();
        }

        public void AddProcessor(IProcessor processor)
        {
            ClusterConfiguration.AddProcessorsToNode(LocalNode, processor);
            Router.Process(new Envelope(new AddProcessorToNode(LocalNode, processor), Priority.Administrative));
        }

        public void RemoveProcessor(IProcessor processor)
        {
            MessageQueue.SuspendMessages(processor.ProcessingType);
            ClusterConfiguration.RemoveProcessorsFromNode(LocalNode, processor);
            Router.Process(new Envelope(new RemoveProcessorFromNode(LocalNode, processor), Priority.Administrative));
            if (LocalNode.Processors.All(o => o.ProcessingType != processor.ProcessingType))
                MessageQueue.RerouteMessages(processor.ProcessingType);
            else
                MessageQueue.ResumeMessages(processor.ProcessingType);
        }

        private void FindClusterAndEstablishCommunication()
        {
            if (_workerConfiguration.ControllerNodes.Empty())
            {
                if (IsController)
                    return;
                throw new NotImplementedException("No Controllers found and I'm not configured as Controller-Node");
            }

            foreach (var controllerNode in _workerConfiguration.ControllerNodes.Where(o => o != LocalNode.Address))
            {
                if (IsControllerOnline(controllerNode))
                {
                    AddLocalNodeToCluster(controllerNode);
                    break;
                }
            }
        }

        private void AddLocalNodeToCluster(Uri controllerNode)
        {
            Router.ProcessSync(new Envelope(new AddNodeToClusterMessage(LocalNode), Priority.Administrative),
                new NodeConfiguration(null, controllerNode));
        }

        private bool IsControllerOnline(Uri controllerNode)
        {
            return Router.ProcessSync(new Envelope(new PingMessage(), Priority.Administrative),
                new NodeConfiguration(null, controllerNode));
        }

        private void AddClusterCommunicationProcessors()
        {
            LocalNode.Processors.Add(new PingProcessor());
            LocalNode.Processors.Add(new SyncClusterConfigurationProcessor());

            if (!IsController)
                return;

            LocalNode.Processors.Add(new RemoveNodeFromClusterProcessor());
            LocalNode.Processors.Add(new AddNodeToClusterProcessor());
            LocalNode.Processors.Add(new BroadcastClusterConfigurationProcessor());
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

            _workingFactory = new SyncMessageWorkFactory(MessageQueue, LocalNode, new Environment(Router, LocalNode.Address) {LocalCluster = ClusterConfiguration});
            _workingFactory.StartWithMessageProcessing();

            Status = WorkerStatus.Starting;
            SetLocalNodeOnline();

            FindClusterAndEstablishCommunication();
            
            Status = WorkerStatus.Started;
        }

        private void SetLocalNodeOnline()
        {
            ClusterConfiguration.AddNode(LocalNode);
        }

        public void Stop()
        {
            if (Status != WorkerStatus.Started)
                throw new Exception("Cannot stop worker, because it is not started");

            Status = WorkerStatus.Stopping;
            _workingFactory.MustCancelWork = false;
            _workingFactory = null;

            SendOfflineMessage();
            SetLocalNodeOffline();
            Status = WorkerStatus.Stopped;
        }

        private void SetLocalNodeOffline()
        {
            ClusterConfiguration.RemoveNode(LocalNode);
        }

        private void SendOfflineMessage()
        {
            Router.Process(new Envelope(new RemoveNodeFromClusterMessage(LocalNode), Priority.Administrative));
        }
    }
}
