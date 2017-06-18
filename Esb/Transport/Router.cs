using System;
using System.Linq;
using Esb.Cluster;
using Esb.Message;

namespace Esb.Transport
{
    public class Router : IRouter
    {
        public Router(IReceiver receiver, IMessageQueue messageQueue, 
            IClusterConfiguration clusterConfiguration, ISender sender,
            INodeRoutingStrategy routingStrategy)
        {
            Receiver = receiver;

            if (Receiver != null)
                Receiver.MessageArrived = Process;

            MessageQueue = messageQueue;
            ClusterConfiguration = clusterConfiguration;
            Sender = sender;
            RoutingStrategy = routingStrategy;
        }
        public void Process(Envelope message)
        {
            if (!ProcessSingleSeverMessage(message))
                if (!ProcessMultiSeverMessage(message))
                    throw new NotImplementedException();
        }

        private bool ProcessMultiSeverMessage(Envelope message)
        {
            var processingNodes = ClusterConfiguration.GetClusterNodesForMessage(message).ToList();
            foreach (var node in processingNodes)
            {
                if (node.IsLocal)
                    MessageQueue.Add(message);
                else
                    Sender.Send(message, node);
            }
            return processingNodes.Any();
        }

        private bool ProcessSingleSeverMessage(Envelope message)
        {
            if (!ClusterConfiguration.IsMultiProcessable(message))
            {
                if (ClusterConfiguration.HasLocalProcessing(message))
                    MessageQueue.Add(message);
                else
                    SentToNodeFromRoutingStrategy(message);
                return true;
            }
            return false;
        }

        private void SentToNodeFromRoutingStrategy(Envelope message)
        {
            if (RoutingStrategy == null)
            {
                Sender.Send(message);
            }
            else
            {
                var processingNodes = ClusterConfiguration.GetClusterNodesForMessage(message);
                var routing = RoutingStrategy.SelectNode(processingNodes);
                Sender.Send(message, routing);
            }
        }

        public IReceiver Receiver { get; }
        public IMessageQueue MessageQueue { get; }
        public IClusterConfiguration ClusterConfiguration { get; }
        public ISender Sender { get; }
        public INodeRoutingStrategy RoutingStrategy { get; }
        /// <summary>
        /// We try  to send the message in Sync and return false if we got an Exception
        /// e.g. if the given node is not online
        /// </summary>
        /// <param name="message"></param>
        /// <param name="targetNode"></param>
        /// <returns></returns>
        public bool ProcessSync(Envelope message, INodeConfiguration targetNode)
        {
            try
            {
                Sender.Send(message, targetNode);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}