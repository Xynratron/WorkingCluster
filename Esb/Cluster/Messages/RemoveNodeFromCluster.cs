using System;

namespace Esb.Cluster.Messages
{
    public class RemoveNodeFromClusterMessage
    {
        public RemoveNodeFromClusterMessage(INodeConfiguration nodeToRemove)
        {
            NodeId = nodeToRemove.NodeId;
        }
        Guid NodeId { get; }
    }
}