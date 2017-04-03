using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Esb.Processing;

namespace Esb.Cluster.Messages
{
    public class AddProcessorToNode
    {
        public AddProcessorToNode(INodeConfiguration node, IProcessor processor)
        {
            Node = node;
            Processor = processor.ProcessingType;
        }
        public INodeConfiguration Node { get; }
        public Type Processor { get; }
    }
}
