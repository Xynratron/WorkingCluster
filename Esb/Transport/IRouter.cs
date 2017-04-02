using System;
using Esb.Cluster;
using Esb.Message;

namespace Esb.Transport
{
    public interface IRouter
    {
        void Process(Envelope message);
        bool ProcessSync(Envelope message, INodeConfiguration targetNode);
    }
}