using System;

namespace Esb.Cluster.Messages
{
    /// <summary>
    /// This Message must only be sent to a single node
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public sealed class SingleProcessingMessageAttribute : Attribute
    {
    }
}