using System;
using System.Collections.Generic;
using System.Linq;
using Esb.Transport;

namespace Esb
{
    public class ClusterConfiguration : IClusterConfiguration
    {
        public List<INodeConfiguration> Nodes { get; } = new List<INodeConfiguration>();

        public void AddNode(INodeConfiguration node)
        {
            lock (Nodes)
            {
                if (Nodes.All(o => o.Address != node.Address))
                    Nodes.Add(node);
            }
        }

        public void RemoveNode(INodeConfiguration node)
        {
            lock (Nodes)
            {
                if (Nodes.Any(o => o.Address == node.Address))
                    Nodes.RemoveAll(o => o.Address == node.Address);
            }
        }

        public void AddProcessorsToNode(INodeConfiguration node, params IProcessor[] processors)
        {
            lock (Nodes)
            {
                foreach (var nodeConfiguration in Nodes.Where(o => o.Address == node.Address))
                {
                    lock (nodeConfiguration.Processors)
                    {
                        foreach (var processor in processors)
                        {
                            nodeConfiguration.Processors.Add(processor);
                        }
                    }
                }
            }
        }

        public void RemoveProcessorsFromNode(INodeConfiguration node, params IProcessor[] processors)
        {
            lock (Nodes)
            {
                foreach (var nodeConfiguration in Nodes.Where(o => o.Address == node.Address))
                {
                    lock (nodeConfiguration.Processors)
                    {
                        foreach (var processor in processors)
                        {
                            nodeConfiguration.Processors.Remove(processor);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Where is "local" defined? A Local processing means within the same proccess, not only the same machine. 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool HasLocalProcessing(Envelope message)
        {
            lock (Nodes)
            {
               return  Nodes.Any(o => o.IsLocal);
            }
        }

        /// <summary>
        /// Returns all nodes which will work with that message
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public IEnumerable<INodeConfiguration> GetClusterNodesForMessage(Envelope message)
        {
            lock (Nodes)
            {
                foreach (var nodeConfiguration in Nodes)
                {
                    if (nodeConfiguration.Processors.Any(o => o == message.MessageType))
                        yield return nodeConfiguration;
                }
            }
        }


        /// <summary>
        /// ToDo Not defined what a Multi is.
        /// For now, we return , if we have more Servers which wan to process this.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool IsMultiProcessable(Envelope message)
        {
            lock (Nodes)
            {
                return Nodes.Select(x => x.Processors.Where(o => o == message.MessageType)).Count() > 1;
            }
        }
    }
}