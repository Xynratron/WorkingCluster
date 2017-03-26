using System;
using System.Collections.Generic;

namespace Esb
{
    public interface INodeConfiguration
    {
        /// <summary>
        /// If a node is part of the current WorkServer. 
        /// </summary>
        bool IsLocal { get; }
        /// <summary>
        /// The Address of the Node, a Address must be unique for the cluster
        /// </summary>
        Uri Address { get; }

        ICollection<IProcessor> Processors { get; }
        Guid NodeId { get; }
    }

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