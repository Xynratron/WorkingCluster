using System;
using Esb.Message;
using Esb.Processing;

namespace Esb.Cluster.Proccessors
{
    public class AskForClusterConfiguration : IProcessor<AskForClusterConfiguration>
    {
        public void Process(IEnvironment environment, Envelope envelope, AskForClusterConfiguration message)
        {
            throw new NotImplementedException();
        }

        public Type ProcessingType => typeof(AskForClusterConfiguration);
        public IProcessor<AskForClusterConfiguration> GetInstance => new AskForClusterConfiguration();
        object IProcessor.GetInstance => GetInstance;
    }
}