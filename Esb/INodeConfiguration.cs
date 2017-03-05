using System;
namespace Esb
{
    public interface INodeConfiguration
    {
        /// <summary>
        /// If a node is part of the current WorkServer. 
        /// </summary>
        bool IsLocal { get; set; }
        /// <summary>
        /// The Address of the Node
        /// </summary>
        Uri Address { get; set; }
    }
}