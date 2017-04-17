using System;
using Esb.Cluster;
using Esb.Message;

namespace Esb.Processing
{
    public interface IProcessor<T> : IProcessor
    {
        void Process(IEnvironment environment, Envelope envelope, T message);
        new IProcessor<T> GetInstance { get; }
    }

    public interface IProcessor
    {
        Type ProcessingType { get; }
        object GetInstance { get; }
    }

    public class ProcessorStubForConfiguration : IProcessor
    {
        public ProcessorStubForConfiguration(Type processingType)
        {
            ProcessingType = processingType;
        }

        public Type ProcessingType { get; }
        public object GetInstance => this;
    }
}