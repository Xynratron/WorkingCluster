using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Esb.Message;
using Esb.Processing;

namespace Esb.Tests.Helper
{
    public class TestMessageProcessor : BaseProcessor<TestMessage>
    {
        public override void Process(IEnvironment environment, Envelope envelope, TestMessage message)
        {
            
        }
    }
    public class BroadcastTestMessageProcessor : BaseProcessor<BroadcastTestMessage>
    {
        public override void Process(IEnvironment environment, Envelope envelope, BroadcastTestMessage message)
        {
            throw new NotImplementedException();
        }
    }
    public class SingleProcessingTestMessageProcessor : BaseProcessor<SingleProcessingTestMessage>
    {
        public override void Process(IEnvironment environment, Envelope envelope, SingleProcessingTestMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
