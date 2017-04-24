using System;
using System.Collections.Generic;
using Esb.Processing;

namespace Esb.Cluster
{
    public class NodeConfiguration : INodeConfiguration
    {
        private readonly IWorker _worker;
        public NodeConfiguration(IWorker worker, Uri address)
        {
            _worker = worker;
            Address = address;
            Processors = new List<IProcessor>();
            IsControllerNode = _worker != null && _worker.IsController;
        }

        public NodeConfiguration(Uri address, bool nodeIsController)
        {
            _worker = null;
            Address = address;
            Processors = new List<IProcessor>();
            IsControllerNode = nodeIsController;
        }
        public bool IsLocal => _worker != null && _worker.LocalNode.Address == Address;
        public Uri Address { get; }
        public ICollection<IProcessor> Processors { get; }
        public bool IsControllerNode { get; }
    }
}