using System;
using Esb.Cluster.Messages;
using Esb.Message;
using Esb.Processing;

namespace Esb.Cluster.Proccessors
{
    public class AddNodeToCluster : IProcessor<AddNodeToCluster>
    {
        public void Process(IEnvironment environment, Envelope envelope, AddNodeToCluster message)
        {
            throw new NotImplementedException();
        }

        public Type ProcessingType => typeof(AddNodeToCluster);

        public IProcessor<AddNodeToCluster> GetInstance => new AddNodeToCluster();
        object IProcessor.GetInstance => GetInstance;
    }
}
