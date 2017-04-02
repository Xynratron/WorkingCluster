using System;
using Esb.Message;
using Esb.Processing;

namespace Esb.Cluster.Proccessors
{
    public class AddProcessorToNode : IProcessor<AddProcessorToNode>
    {
        public Type ProcessingType { get; }
        public void Process(IEnvironment environment, Envelope envelope, AddProcessorToNode message)
        {
            throw new NotImplementedException();
        }

        public IProcessor<AddProcessorToNode> GetInstance => new AddProcessorToNode();
        object IProcessor.GetInstance => GetInstance;
    }
}