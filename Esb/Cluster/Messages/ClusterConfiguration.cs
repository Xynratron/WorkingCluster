using System;
using System.Collections.Generic;

namespace Esb.Cluster.Messages
{
    [BroadcastProcessingMessage]
    public class ClusterConfigurationMessage
    {
        public ClusterConfigurationMessage(Uri sender, IEnumerable<INodeConfiguration> nodes)
        {
            Sender = sender;
            Nodes = nodes;
        }

        public Uri Sender { get; }
        public IEnumerable<INodeConfiguration> Nodes { get; }
    }
}