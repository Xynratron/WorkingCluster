using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Esb.Transport
{
    public interface INodeRoutingStrategy
    {
        INodeConfiguration SelectNode(List<INodeConfiguration> nodes);
    }
 }
