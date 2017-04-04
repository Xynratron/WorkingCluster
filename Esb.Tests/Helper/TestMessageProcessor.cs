using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Esb.Message;
using Esb.Processing;

namespace Esb.Tests.Helper
{
    public class TestMessageProcessor : IProcessor<TestMessage>
    {
        public Type ProcessingType => typeof(TestMessage);
        public void Process(IEnvironment environment, Envelope envelope, TestMessage message)
        {
            throw new NotImplementedException();
        }
        public IProcessor<TestMessage> GetInstance => new TestMessageProcessor();
        object IProcessor.GetInstance => GetInstance;
    }
}
