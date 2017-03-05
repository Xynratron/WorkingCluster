using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Esb.Transport
{
    public class Router : IRouter
    {
        public Router(IReceiver receiver, IMessageQueue messageQueue, 
            IClusterConfiguration clusterConfiguration, ISender sender,
            INodeRoutingStrategy routingStrategy)
        {
            Receiver = receiver;
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
            var processingNodes = ClusterConfiguration.GetClusterNodesForMessage(message);
            foreach (var node in ClusterConfiguration.GetClusterNodesForMessage(message))
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
                    Sender.Send(message, GetNodeFromRoutingStrategy(message));
                return true;
            }
            return false;
        }

        private INodeConfiguration GetNodeFromRoutingStrategy(Envelope message)
        {
            var processingNodes = ClusterConfiguration.GetClusterNodesForMessage(message);
            return RoutingStrategy.SelectNode(processingNodes);
        }

        public IReceiver Receiver { get; }
        public IMessageQueue MessageQueue { get; }
        public IClusterConfiguration ClusterConfiguration { get; }
        public ISender Sender { get; }
        public INodeRoutingStrategy RoutingStrategy { get; }
         
    }
}