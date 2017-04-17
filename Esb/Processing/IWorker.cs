using Esb.Cluster;
using Esb.Message;
using Esb.Transport;

namespace Esb.Processing
{
    public interface IWorker
    {
        INodeConfiguration LocalNode { get; }
        IRouter Router { get;  }
        IMessageQueue MessageQueue { get; }
        IClusterConfiguration ClusterConfiguration { get; }
        bool IsController { get; }
        WorkerStatus Status { get; }
        void Start();
        void Stop();
    }
}