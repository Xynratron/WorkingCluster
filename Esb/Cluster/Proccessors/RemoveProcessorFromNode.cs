using System;
using Esb.Message;
using Esb.Processing;

namespace Esb.Cluster.Proccessors
{
    public class RemoveProcessorFromNode : IProcessor<RemoveProcessorFromNode>
    {
        public Type ProcessingType => typeof(RemoveProcessorFromNode);
        public void Process(IEnvironment environment, Envelope envelope, RemoveProcessorFromNode message)
        {
            throw new NotImplementedException();
        }
        public IProcessor<RemoveProcessorFromNode> GetInstance => new RemoveProcessorFromNode();
        object IProcessor.GetInstance => GetInstance;
    }
}