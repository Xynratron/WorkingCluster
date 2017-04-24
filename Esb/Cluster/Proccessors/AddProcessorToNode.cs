using System;
using Esb.Cluster.Messages;
using Esb.Message;
using Esb.Processing;

namespace Esb.Cluster.Proccessors
{
    public class AddProcessorToNodeProcessor : IProcessor<AddProcessorToNode>
    {
        
        public void Process(IEnvironment environment, Envelope envelope, AddProcessorToNode message)
        {
            environment.Logger.Debug(envelope, "Start of AddProcessorToNodeProcessor.Process");

            environment.Logger.Debug(envelope, $"Adding processor for types of {message.Processor} to node {message.Node.Address}.");
            environment.LocalCluster.AddProcessorsToNode(message.Node, message.Processor);
            
            environment.Process(new Envelope(new BroadcastClusterConfigurationProcessor(), Priority.Administrative));

            environment.Logger.Debug(envelope, "End of AddProcessorToNodeProcessor.Process");
        }

        public Type ProcessingType => typeof(AddProcessorToNode);
        public IProcessor<AddProcessorToNode> GetInstance => new AddProcessorToNodeProcessor();
        object IProcessor.GetInstance => GetInstance;
    }
}