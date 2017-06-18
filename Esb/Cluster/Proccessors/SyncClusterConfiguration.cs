using System;
using System.Linq;
using Esb.Cluster.Messages;
using Esb.Message;
using Esb.Processing;

namespace Esb.Cluster.Proccessors
{
    public class SyncClusterConfigurationProcessor : IProcessor<ClusterConfigurationMessage>
    {
        public void Process(IEnvironment environment, Envelope envelope, ClusterConfigurationMessage message)
        {
            environment.Logger.Debug(envelope, "Start of SyncClusterConfigurationProcessor");

            if (message.Sender == environment.LocalAddress)
            {
                environment.Logger.Debug(envelope, "Message was sent from local node, we ignore it.");
                return;
            }
            foreach (var clusterNode in message.Nodes)
            {
                environment.LocalCluster.AddNode(clusterNode);
                environment.LocalCluster.AddProcessorsToNode(clusterNode, clusterNode.Processors.ToArray());
            }

            environment.Logger.Debug(envelope, "End of SyncClusterConfigurationProcessor");
        }

        public Type ProcessingType => typeof(ClusterConfigurationMessage);
        public IProcessor<ClusterConfigurationMessage> GetInstance => new SyncClusterConfigurationProcessor();
        object IProcessor.GetInstance => GetInstance;
    }
}