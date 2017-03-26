using System.Collections.Generic;

namespace Esb.Transport
{
    public interface INodeRoutingStrategy
    {
        INodeConfiguration SelectNode(IEnumerable<INodeConfiguration> nodes);
    }
}
