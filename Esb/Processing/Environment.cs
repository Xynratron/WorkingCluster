using Esb.Cluster;

namespace Esb.Processing
{
    public class Environment : IEnvironment
    {
        public IClusterConfiguration LocalCluster { get; set; }
    }
}