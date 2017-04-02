using System;
using System.Collections.Generic;
using Esb.Processing;

namespace Esb.Cluster
{
    public class NodeConfiguration : INodeConfiguration
    {
        private readonly IWorker _worker;
        public NodeConfiguration(IWorker worker, Guid nodeId, Uri address)
        {
            _worker = worker;
            Address = address;
            Processors = new List<IProcessor>();
            NodeId = nodeId;
        }

        public bool IsLocal => _worker.LocalNode.NodeId == NodeId;
        public Uri Address { get; }
        public ICollection<IProcessor> Processors { get; }
        public Guid NodeId { get; }
    }
}