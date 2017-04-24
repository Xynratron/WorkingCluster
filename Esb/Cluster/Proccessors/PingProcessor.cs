using System;
using Esb.Cluster.Messages;
using Esb.Message;
using Esb.Processing;

namespace Esb.Cluster.Proccessors
{
    public class PingProcessor : IProcessor<PingMessage>
    {
        public void Process(IEnvironment environment, Envelope envelope, PingMessage message)
        {
            environment.Logger.Debug(envelope, "Start of PingProcessor");

            environment.Logger.Debug(envelope, "Got a Ping, nothing to do here, maybe we implement a Pong also.");

            environment.Logger.Debug(envelope, "End of PingProcessor");
        }

        public Type ProcessingType => typeof(PingMessage);
        public IProcessor<PingMessage> GetInstance => new PingProcessor();
        object IProcessor.GetInstance => GetInstance;
    }
}