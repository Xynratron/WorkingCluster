using System;

namespace Esb.ClusterCommunication
{
    public class RemoveNodeFromCluster : IProcessor<RemoveNodeFromCluster>
    {
        public void Process(IEnvironment environment, Envelope envelope, RemoveNodeFromCluster message)
        {
            throw new NotImplementedException();
        }

        public Type ProcessingType => typeof(RemoveNodeFromCluster);
        object IProcessor.GetInstance => GetInstance;
        public RemoveNodeFromCluster GetInstance => new RemoveNodeFromCluster();
    }
}