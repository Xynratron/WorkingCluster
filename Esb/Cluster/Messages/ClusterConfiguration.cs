using System;
using System.Collections.Generic;

namespace Esb.Cluster.Messages
{
    [BroadcastProcessingMessage]
    public class ClusterConfigurationMessage
    {
        public Uri Sender { get; }
        public ICollection<INodeConfiguration> Nodes { get; }
    }
}