using System;
using Esb.Cluster.Messages;
using Esb.Message;
using Esb.Processing;

namespace Esb.Cluster.Proccessors
{
    public class SyncClusterConfigurationProcessor : IProcessor<ClusterConfigurationMessage>
    {
        public void Process(IEnvironment environment, Envelope envelope, ClusterConfigurationMessage message)
        {
            throw new NotImplementedException();
        }

        public Type ProcessingType => typeof(ClusterConfigurationMessage);
        public IProcessor<ClusterConfigurationMessage> GetInstance => new SyncClusterConfigurationProcessor();
        object IProcessor.GetInstance => GetInstance;
    }
}