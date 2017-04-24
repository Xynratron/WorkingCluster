using System;
using Esb.Cluster.Messages;
using Esb.Message;
using Esb.Processing;

namespace Esb.Cluster.Proccessors
{
    public class BroadcastClusterConfigurationProcessor : IProcessor<BroadcastClusterConfigurationMessage>
    {
        public void Process(IEnvironment environment, Envelope envelope, BroadcastClusterConfigurationMessage message)
        {
            environment.Logger.Debug(envelope, "Start of BroadcastClusterConfigurationProcessor");

            var cc = new ClusterConfigurationMessage(environment.LocalAddress, environment.LocalCluster.Nodes);
            environment.Process(new Envelope(cc, Priority.Administrative, envelope.TransactionId));

            environment.Logger.Debug(envelope, "End of BroadcastClusterConfigurationProcessor");
        }

        public Type ProcessingType => typeof(BroadcastClusterConfigurationMessage);
        public IProcessor<BroadcastClusterConfigurationMessage> GetInstance => new BroadcastClusterConfigurationProcessor();
        object IProcessor.GetInstance => GetInstance;
    }
}