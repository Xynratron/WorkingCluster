using Esb.Cluster;

namespace Esb.Processing
{
    public interface IWorker
    {
        INodeConfiguration LocalNode { get; }
        bool IsController { get; }

        WorkerStatus Status { get; }

        void Start();
        void Stop();
    }
}