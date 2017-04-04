using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Esb.Message;

namespace Esb.Processing
{
    public abstract class BaseProcessor<T> : IProcessor<T>
    {
        public virtual Type ProcessingType => typeof(T);
        public abstract void Process(IEnvironment environment, Envelope envelope, T message);
        public virtual IProcessor<T> GetInstance => (IProcessor<T>)Activator.CreateInstance(this.GetType());
        object IProcessor.GetInstance => GetInstance;
    }
}
