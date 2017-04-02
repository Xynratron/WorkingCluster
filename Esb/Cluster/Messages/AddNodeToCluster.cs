using System;
using System.Collections.Generic;
using System.Linq;

namespace Esb.Cluster.Messages
{
    public class AddNodeToCluster
    {
        public AddNodeToCluster(INodeConfiguration nodeToAdd)
        {
            Address = nodeToAdd.Address;
            NodeId = nodeToAdd.NodeId;
            Processors = new List<Type>(nodeToAdd.Processors.Select(o => o.ProcessingType));
        }
        Uri Address { get; }
        ICollection<Type> Processors { get; }
        Guid NodeId { get; }
    }
}
