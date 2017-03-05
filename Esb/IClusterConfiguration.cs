using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace Esb
{
    public interface IClusterConfiguration
    {
        void AddNode(INodeConfiguration node);
        void RemoveNode(INodeConfiguration node);
        void AddProcessorsToNode(INodeConfiguration node, List<IProcessor> processors);
        void RemoveProcessorsFromNode(INodeConfiguration node, List<IProcessor> processors);
        bool HasLocalProcessing(Envelope message);
        IEnumerable<INodeConfiguration> GetClusterNodesForMessage(Envelope message);
        bool IsMultiProcessable(Envelope message);
    }

    public interface IProcessor
    {
        Type ProcessingType { get; set; }
    }
}