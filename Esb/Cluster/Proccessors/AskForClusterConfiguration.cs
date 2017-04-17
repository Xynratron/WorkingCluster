using System;
using Esb.Cluster.Messages;
using Esb.Message;
using Esb.Processing;

namespace Esb.Cluster.Proccessors
{
    public class AskForClusterConfigurationProcessor : IProcessor<AskForClusterConfiguration>
    {
        public void Process(IEnvironment environment, Envelope envelope, AskForClusterConfiguration message)
        {
            environment.Logger.Debug(envelope, "Start of AskForClusterConfigurationProcessor");

            var cc = new ClusterConfigurationMessage(environment.LocalAddress, environment.LocalCluster.Nodes);
            environment.Process(new Envelope(cc, Priority.Administrative, envelope.TransactionId));

            environment.Logger.Debug(envelope, "End of AskForClusterConfigurationProcessor");
        }

        public Type ProcessingType => typeof(AskForClusterConfiguration);
        public IProcessor<AskForClusterConfiguration> GetInstance => new AskForClusterConfigurationProcessor();
        object IProcessor.GetInstance => GetInstance;
    }
}