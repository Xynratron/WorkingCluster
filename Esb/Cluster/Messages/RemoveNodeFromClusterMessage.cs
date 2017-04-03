using System;

namespace Esb.Cluster.Messages
{
    public class RemoveNodeFromClusterMessage
    {
        public RemoveNodeFromClusterMessage(INodeConfiguration nodeToRemove)
        {
            Address = nodeToRemove.Address;
        }
        public Uri Address { get; set; }
    }
}