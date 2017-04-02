using System;

namespace Esb.ClusterCommunication.Messages
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