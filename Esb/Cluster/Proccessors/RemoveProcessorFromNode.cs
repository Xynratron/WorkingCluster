using System;
using Esb.Cluster.Messages;
using Esb.Message;
using Esb.Processing;

namespace Esb.Cluster.Proccessors
{
    public class RemoveProcessorFromNodeProcessor : IProcessor<RemoveProcessorFromNode>
    {
        public Type ProcessingType => typeof(RemoveProcessorFromNode);
        public void Process(IEnvironment environment, Envelope envelope, RemoveProcessorFromNode message)
        {
            throw new NotImplementedException();
        }
        public IProcessor<RemoveProcessorFromNode> GetInstance => new RemoveProcessorFromNodeProcessor();
        object IProcessor.GetInstance => GetInstance;
    }
}