using System;
using Esb.Message;
using Esb.Processing;

namespace Esb.Cluster.Proccessors
{
    public class RemoveNodeFromCluster : IProcessor<RemoveNodeFromCluster>
    {
        public void Process(IEnvironment environment, Envelope envelope, RemoveNodeFromCluster message)
        {
            throw new NotImplementedException();
        }

        public Type ProcessingType => typeof(RemoveNodeFromCluster);
        object IProcessor.GetInstance => GetInstance;
        public IProcessor<RemoveNodeFromCluster> GetInstance => new RemoveNodeFromCluster();
    }
}