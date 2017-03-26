using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace Esb
{
    public interface IClusterConfiguration
    {
        void AddNode(INodeConfiguration node);
        void RemoveNode(INodeConfiguration node);
        void AddProcessorsToNode(INodeConfiguration node, params IProcessor[] processors);
        void RemoveProcessorsFromNode(INodeConfiguration node, params IProcessor[] processors);
        bool HasLocalProcessing(Envelope message);
        IEnumerable<INodeConfiguration> GetClusterNodesForMessage(Envelope message);
        bool IsMultiProcessable(Envelope message);
    }
}