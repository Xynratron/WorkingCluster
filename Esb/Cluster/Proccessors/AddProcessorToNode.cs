using System;
using Esb.Cluster.Messages;
using Esb.Message;
using Esb.Processing;

namespace Esb.Cluster.Proccessors
{
    public class AddProcessorToNodeProcessor : IProcessor<AddProcessorToNode>
    {
        public Type ProcessingType { get; }
        public void Process(IEnvironment environment, Envelope envelope, AddProcessorToNode message)
        {
            environment.LocalCluster.AddProcessorsToNode(message.Node, message.Processor);
        }

        public IProcessor<AddProcessorToNode> GetInstance => new AddProcessorToNodeProcessor();
        object IProcessor.GetInstance => GetInstance;
    }
}