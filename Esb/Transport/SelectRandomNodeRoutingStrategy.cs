using System;
using System.Collections.Generic;
using System.Linq;
using Esb.Cluster;

namespace Esb.Transport
{
    public class SelectRandomNodeRoutingStrategy : INodeRoutingStrategy
    {
        public static Random Random = new Random();
        public INodeConfiguration SelectNode(IEnumerable<INodeConfiguration> nodes)
        {
            if (nodes == null)
                throw new ArgumentNullException(nameof(nodes));
            nodes = nodes.ToList();
            if (!nodes.Any())
                throw new ArgumentOutOfRangeException();

            var rnd = Random.Next(nodes.Count()-1);
            return nodes.Skip(rnd).First();
        }
    }
}