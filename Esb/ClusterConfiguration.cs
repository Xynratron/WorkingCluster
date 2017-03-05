using System;
using System.Collections.Generic;
using System.Linq;
using Esb.Transport;

namespace Esb
{
    public class ClusterConfiguration : IClusterConfiguration
    {
        private readonly List<INodeConfiguration> _nodes = new List<INodeConfiguration>();

        public void AddNode(INodeConfiguration node)
        {
            lock (_nodes)
            {
                if (_nodes.All(o => o.Address != node.Address))
                    _nodes.Add(node);
            }
        }

        public void RemoveNode(INodeConfiguration node)
        {
            lock (_nodes)
            {
                if (_nodes.Any(o => o.Address == node.Address))
                    _nodes.RemoveAll(o => o.Address == node.Address);
            }
        }

        public void AddProcessorsToNode(INodeConfiguration node, List<IProcessor> processors)
        {
            lock (_nodes)
            {
                foreach (var nodeConfiguration in _nodes.Where(o => o.Address == node.Address))
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

        public void RemoveProcessorsFromNode(INodeConfiguration node, List<IProcessor> processors)
        {
            lock (_nodes)
            {
                foreach (var nodeConfiguration in _nodes.Where(o => o.Address == node.Address))
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
            lock (_nodes)
            {
               return  _nodes.Any(o => o.IsLocal);
            }
        }

        /// <summary>
        /// Returns all nodes which will work with that message
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public IEnumerable<INodeConfiguration> GetClusterNodesForMessage(Envelope message)
        {
            lock (_nodes)
            {
                foreach (var nodeConfiguration in _nodes)
                {
                    if (nodeConfiguration.Processors.Any(o => o.ProcessingType == message.MessageType))
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
            lock (_nodes)
            {
                return _nodes.Select(x => x.Processors.Where(o => o.ProcessingType == message.MessageType)).Count() > 1;
            }
        }
    }
}