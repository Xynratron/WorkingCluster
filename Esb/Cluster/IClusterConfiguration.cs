using System;
using System.Collections.Generic;
using Esb.Message;
using Esb.Processing;

namespace Esb.Cluster
{
    public interface IClusterConfiguration
    {
        void AddNode(INodeConfiguration node);
        void RemoveNode(INodeConfiguration node);
        void AddProcessorsToNode(INodeConfiguration node, params IProcessor[] processors);
        void RemoveProcessorsFromNode(INodeConfiguration node, params IProcessor[] processors);
        void AddProcessorsToNode(INodeConfiguration node, params Type[] processors);
        void RemoveProcessorsFromNode(INodeConfiguration node, params Type[] processors);
        bool HasLocalProcessing(Envelope message);
        IEnumerable<INodeConfiguration> GetClusterNodesForMessage(Envelope message);
        bool IsMultiProcessable(Envelope message);
    }
}