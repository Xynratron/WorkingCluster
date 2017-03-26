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
}