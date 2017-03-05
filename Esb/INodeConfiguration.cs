using System;
using System.Collections.Generic;

namespace Esb
{
    public interface INodeConfiguration
    {
        /// <summary>
        /// If a node is part of the current WorkServer. 
        /// HowTo ToDo What is lokal? Who defines taht?
        /// </summary>
        bool IsLocal { get; set; }
        /// <summary>
        /// The Address of the Node
        /// </summary>
        Uri Address { get; set; }

        ICollection<IProcessor> Processors { get; set; }
    }
}