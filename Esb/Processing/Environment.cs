using System;
using Esb.Cluster;
using Esb.Message;
using Esb.Transport;
using NLog;

namespace Esb.Processing
{
    public class Environment : IEnvironment
    {
        public IRouter LocalRouter { get; }

        public Environment(IRouter localRouter, Uri localAddress)
        {
            Logger = new EnvironmentLogger();
            LocalRouter = localRouter;
            LocalAddress = localAddress;
        }
        public IClusterConfiguration LocalCluster { get; set; }
        public ILog Logger { get; set; }
        public void Process(Envelope message)
        {
            LocalRouter.Process(message);
        }

        public bool ProcessSync(Envelope message, INodeConfiguration targetNode)
        {
            return LocalRouter.ProcessSync(message, targetNode);
        }

        public Uri LocalAddress { get; }
    }

    public interface IEnvironment
    {
        //ToDo rename to ClusterConfiguration, because it is not local
        IClusterConfiguration LocalCluster { get; }
        ILog Logger { get; set; }
        void Process(Envelope message);
        bool ProcessSync(Envelope message, INodeConfiguration targetNode);
        Uri LocalAddress { get; }
    }

    public interface ILog
    {
        void Debug(Envelope envelope, string message);
        void Error(Envelope envelope, string message);
        void Error(Envelope envelope, Exception exception, string message);
        void Info(Envelope envelope, string message);
        void Trace(Envelope envelope, string message); 
        void Warn(Envelope envelope, string message);
        void Warn(Envelope envelope, Exception exception, string message);
    }

    public class EnvironmentLogger : ILog
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public void Debug(Envelope envelope, string message)
        {
            Logger.Debug(message);
        }

        public void Error(Envelope envelope, string message)
        {
            Logger.Error(message);
        }

        public void Error(Envelope envelope, Exception exception, string message)
        {
            Logger.Error(exception, message);
        }

        public void Info(Envelope envelope, string message)
        {
            Logger.Info(message);
        }

        public void Trace(Envelope envelope, string message)
        {
            Logger.Trace(message);
        }

        public void Warn(Envelope envelope, string message)
        {
            Logger.Warn(message);
        }

        public void Warn(Envelope envelope, Exception exception, string message)
        {
            Logger.Warn(exception, message);
        }
    }
}