using System;
using System.Collections.Generic;
using Esb.Processing;

namespace Esb.Cluster
{
    public interface INodeConfiguration
    {
        /// <summary>
        /// If a node is part of the current WorkServer. This should only map to the locl worker instance and must be ignored by other workers.
        /// </summary>
        bool IsLocal { get; }

        /// <summary>
        /// The Address of the Node, a Address must be unique for the cluster
        /// </summary>
        Uri Address { get; }

        /// <summary>
        /// All Processors which can run on this node.
        /// </summary>
        ICollection<IProcessor> Processors { get; }
        /// <summary>
        /// a unique id of the node, maybe to delete, because we have the address as unique id.
        /// </summary>
        [Obsolete("we have the node address as unique id")]
        Guid NodeId { get; }
        /// <summary>
        /// This Node is part of the Cluster Coucil
        /// </summary>
        bool IsControllerNode { get; }
    }
}