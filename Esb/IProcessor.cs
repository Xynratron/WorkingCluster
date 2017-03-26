using System;
using System.CodeDom;
using System.Security.Cryptography.X509Certificates;

namespace Esb
{
    public interface IProcessor<T> : IProcessor
    {
        void Process(IEnvironment environment, Envelope envelope, T message);
    }

    public interface IProcessor
    {
        Type ProcessingType { get; }
    }

    public interface IEnvironment
    {
    }
}