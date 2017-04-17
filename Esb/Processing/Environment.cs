using System;
using Esb.Cluster;
using Esb.Message;
using NLog;

namespace Esb.Processing
{
    public class Environment : IEnvironment
    {
        public IClusterConfiguration LocalCluster { get; set; }
        public ILog Logger { get; set; }
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
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public void Debug(Envelope envelope, string message)
        {
            logger.Debug(message);
        }

        public void Error(Envelope envelope, string message)
        {
            logger.Error(message);
        }

        public void Error(Envelope envelope, Exception exception, string message)
        {
            logger.Error(exception, message);
        }

        public void Info(Envelope envelope, string message)
        {
            logger.Info(message);
        }

        public void Trace(Envelope envelope, string message)
        {
            logger.Trace(message);
        }

        public void Warn(Envelope envelope, string message)
        {
            logger.Warn(message);
        }

        public void Warn(Envelope envelope, Exception exception, string message)
        {
            logger.Warn(exception, message);
        }
    }
}