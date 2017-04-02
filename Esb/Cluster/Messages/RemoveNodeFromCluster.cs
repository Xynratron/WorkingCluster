using System;

namespace Esb.Cluster.Messages
{
    public class RemoveNodeFromCluster
    {
        public RemoveNodeFromCluster(INodeConfiguration nodeToRemove)
        {
            NodeId = nodeToRemove.NodeId;
        }
        Guid NodeId { get; }
    }
}