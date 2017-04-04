using System;

namespace Esb.Cluster.Messages
{
    /// <summary>
    /// This Message must be sent all nodes, which have a processor for this node.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public sealed class BroadcastProcessingMessageAttribute : Attribute
    {
    }
}