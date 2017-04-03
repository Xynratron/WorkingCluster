using System;
using Esb.Cluster.Messages;
using Esb.Message;
using Esb.Processing;

namespace Esb.Cluster.Proccessors
{
    public class RemoveNodeFromClusterProcessor : IProcessor<RemoveNodeFromClusterMessage>
    {
        public void Process(IEnvironment environment, Envelope envelope, RemoveNodeFromClusterMessage message)
        {
            throw new NotImplementedException();
        }

        public Type ProcessingType => typeof(RemoveNodeFromClusterMessage);
        object IProcessor.GetInstance => GetInstance;
        public IProcessor<RemoveNodeFromClusterMessage> GetInstance => new RemoveNodeFromClusterProcessor();
    }
}