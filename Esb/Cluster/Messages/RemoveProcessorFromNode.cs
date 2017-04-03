using System;
using Esb.Processing;

namespace Esb.Cluster.Messages
{
    public class RemoveProcessorFromNode
    {
        public RemoveProcessorFromNode(INodeConfiguration node, IProcessor processor)
        {
            Node = node;
            Processor = processor.ProcessingType;
        }
        public INodeConfiguration Node { get; }
        public Type Processor { get; }
    }
}