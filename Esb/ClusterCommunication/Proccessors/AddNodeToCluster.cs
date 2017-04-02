using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Esb.ClusterCommunication.Messages;

namespace Esb.ClusterCommunication
{
    public class AddNodeToCluster : IProcessor<AddNodeToCluster>
    {
        public void Process(IEnvironment environment, Envelope envelope, AddNodeToCluster message)
        {
            throw new NotImplementedException();
        }

        public Type ProcessingType => typeof(AddNodeToCluster);

        public AddNodeToCluster GetInstance => new AddNodeToCluster();
        object IProcessor.GetInstance => GetInstance;
    }
}
