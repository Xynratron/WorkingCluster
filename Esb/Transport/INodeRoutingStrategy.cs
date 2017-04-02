using System.Collections.Generic;
using Esb.Cluster;

namespace Esb.Transport
{
    public interface INodeRoutingStrategy
    {
        INodeConfiguration SelectNode(IEnumerable<INodeConfiguration> nodes);
    }
}
