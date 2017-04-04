using Esb.Cluster.Messages;

namespace Esb.Tests.Helper
{
    public class TestMessage { }

    [BroadcastProcessingMessage]
    public class BroadcastTestMessage { }

    [SingleProcessingMessage]
    public class SingleProcessingTestMessage { }
}