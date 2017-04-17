using System;
using Esb.Cluster;
using Esb.Message;
using NLog;

namespace Esb.Processing
{
    public class Environment : IEnvironment
    {
        public Environment()
        {
            Logger = new EnvironmentLogger();
        }
        public IClusterConfiguration LocalCluster { get; set; }
        public ILog Logger { get; set; }
    }

    public interface IEnvironment
    {
        IClusterConfiguration LocalCluster { get; }
        ILog Logger { get; set; }
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