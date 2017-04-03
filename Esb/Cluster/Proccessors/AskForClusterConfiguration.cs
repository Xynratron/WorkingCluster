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
            throw new NotImplementedException();
        }

        public Type ProcessingType => typeof(AskForClusterConfiguration);
        public IProcessor<AskForClusterConfiguration> GetInstance => new AskForClusterConfigurationProcessor();
        object IProcessor.GetInstance => GetInstance;
    }
}