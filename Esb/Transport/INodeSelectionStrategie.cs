using System.CodeDom;
using System.Collections.Generic;
using System.Text;

namespace Esb.Transport
{
    public interface INodeRoutingStrategy
    {
        INodeConfiguration SelectNode(IEnumerable<INodeConfiguration> nodes);
    }
}
