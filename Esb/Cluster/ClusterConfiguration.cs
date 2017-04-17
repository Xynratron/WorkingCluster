using System;
using System.Collections.Generic;
using System.Linq;
using Esb.Cluster.Messages;
using Esb.Message;
using Esb.Processing;

namespace Esb.Cluster
{
    public class ClusterConfiguration : IClusterConfiguration
    {
        private readonly List<INodeConfiguration> _nodes = new List<INodeConfiguration>();

        public IEnumerable<INodeConfiguration> Nodes => _nodes;

        public void AddNode(INodeConfiguration node)
        {
            lock (Nodes)
            {
                if (Nodes.All(o => o.Address != node.Address))
                    _nodes.Add(node);
            }
        }

        public void RemoveNode(INodeConfiguration node)
        {
            lock (Nodes)
            {
                if (Nodes.Any(o => o.Address == node.Address))
                    _nodes.RemoveAll(o => o.Address == node.Address);
            }
        }

        public void AddProcessorsToNode(INodeConfiguration node, params IProcessor[] processors)
        {
            lock (Nodes)
            {
                var nodeConfiguration = Nodes.First(o => o.Address == node.Address);

                lock (nodeConfiguration.Processors)
                {
                    foreach (var processor in processors)
                    {
                        if (nodeConfiguration.Processors.Any(o => processor.GetType() == o.GetType()))
                            continue;
                        nodeConfiguration.Processors.Add(processor);
                    }
                }
            }
        }

        public void AddProcessorsToNode(INodeConfiguration node, params Type[] processors)
        {
            lock (Nodes)
            {
                AddProcessorsToNode(node, processors.Select(Activator.CreateInstance).Cast<IProcessor>().ToArray());
            }
        }

        public void RemoveProcessorsFromNode(INodeConfiguration node, params IProcessor[] processors)
        {
            lock (Nodes)
            {
                RemoveProcessorsFromNode(node, processors.Select(o => o.GetType()).ToArray());
            }
        }

        public void RemoveProcessorsFromNode(INodeConfiguration node, params Type[] processors)
        {
            lock (Nodes)
            {
                var nodeConfiguration = Nodes.First(o => o.Address == node.Address);

                lock (nodeConfiguration.Processors)
                {
                    foreach (var processor in processors)
                    {
                        foreach (var nodeProcessor in nodeConfiguration.Processors.Where(o => processor == o.GetType()).ToList())
                        {
                            nodeConfiguration.Processors.Remove(nodeProcessor);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Indicates if the local node hat a processor for the message.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool HasLocalProcessing(Envelope message)
        {
            lock (Nodes)
            {
                var localNode = Nodes.FirstOrDefault(o => o.IsLocal);
                return  localNode != null && localNode.Processors.Any(o => o.ProcessingType == message.MessageType);
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
                    if (nodeConfiguration.Processors.Any(o => o.ProcessingType == message.MessageType))
                        yield return nodeConfiguration;
                }
            }
        }


        /// <summary>
        /// Detects the Attribute for MultiProcessing
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool IsMultiProcessable(Envelope message)
        {
            lock (Nodes)
            {
                return message.MessageType.GetCustomAttributes(typeof(BroadcastProcessingMessageAttribute), true).Any();
            }
        }
    }
}