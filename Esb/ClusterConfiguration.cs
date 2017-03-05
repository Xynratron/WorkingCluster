using System;
using System.Collections.Generic;

namespace Esb
{
    public class ClusterConfiguration : IClusterConfiguration
    {
        public void AddNode(INodeConfiguration node)
        {
            throw new NotImplementedException();
        }

        public void RemoveNode(INodeConfiguration node)
        {
            throw new NotImplementedException();
        }

        public void AddProcessorsToNode(INodeConfiguration node, List<IProcessor> processors)
        {
            throw new NotImplementedException();
        }

        public void RemoveProcessorsFromNode(INodeConfiguration node, List<IProcessor> processors)
        {
            throw new NotImplementedException();
        }

        public bool HasLocalProcessing(Envelope message)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<INodeConfiguration> GetClusterNodesForMessage(Envelope message)
        {
            throw new NotImplementedException();
        }

        public bool IsMultiProcessable(Envelope message)
        {
            throw new NotImplementedException();
        }
    }
}