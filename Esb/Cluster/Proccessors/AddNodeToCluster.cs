using System;
using System.Linq;
using Esb.Cluster.Messages;
using Esb.Message;
using Esb.Processing;

namespace Esb.Cluster.Proccessors
{
    public class AddNodeToClusterProcessor : IProcessor<AddNodeToCluster>
    {
        public void Process(IEnvironment environment, Envelope envelope, AddNodeToCluster message)
        {
            environment.Logger.Debug(envelope, "Start of AddNodeToClusterProcessor.Process");

            var node = new NodeConfiguration(message.Address, message.IsControllerNode);
            foreach (var messageProcessorType in message.Processors)
            {
                node.Processors.Add(new ProcessorStubForConfiguration(messageProcessorType));
                environment.Logger.Debug(envelope, $"Adding processor for types of {messageProcessorType} to node.");
            }
            environment.LocalCluster.AddNode(node);

            environment.Process(new Envelope(new BroadcastClusterConfigurationProcessor(), Priority.Administrative));

            environment.Logger.Debug(envelope, "End of AddNodeToClusterProcessor.Process");
        }

        public Type ProcessingType => typeof(AddNodeToCluster);

        public IProcessor<AddNodeToCluster> GetInstance => new AddNodeToClusterProcessor();
        object IProcessor.GetInstance => GetInstance;
    }
}
