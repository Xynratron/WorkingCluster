using System;
using Esb.Cluster;
using Esb.Message;
using Esb.Tests.Helper;
using Esb.Transport;
using NUnit.Framework;
using Telerik.JustMock;
using Telerik.JustMock.Helpers;
using MyRouter = Esb.Transport.Router;


namespace Esb.Tests.Transport
{
    [TestFixture()]
    public class MyRouterTests
    {

        [Test()]
        public void SingleServerMessageWithLocalProcessingShouldEndInMessageQueue()
        {
            var message = new Envelope(new TestMessage());

            var messageQueue = Mock.Create<IMessageQueue>();
            messageQueue.Arrange(o => o.Add(message)).MustBeCalled();

            var clusterConfig = ClusterConfiguration(message, true, false);

            var router = new MyRouter(null, messageQueue, clusterConfig, null, null);
            router.Process(message);

            messageQueue.Assert();

        }

        [Test()]
        public void SingleServerMessageWithoutLocalProcessingShouldBeSentToAnotherNode()
        {
            var message = new Envelope(new TestMessage());

            var sender = Mock.Create<ISender>();
            sender.Arrange(o => o.Send(message)).MustBeCalled();
            
            var clusterConfig = ClusterConfiguration(message, false, false);

            var router = new MyRouter(null, null, clusterConfig, sender, null);
            router.Process(message);

            sender.Assert();
        }

        [Test()]
        public void MultiServerMessageWithoutLocalProcessingShouldBeSentAnyOtherNode()
        {
            var message = new Envelope(new TestMessage());
            var node1 = Mock.Create<INodeConfiguration>();
            var node2 = Mock.Create<INodeConfiguration>();

            var sender = Mock.Create<ISender>();
            sender.Arrange(o => o.Send(message, node1)).MustBeCalled();
            sender.Arrange(o => o.Send(message, node2)).MustBeCalled();

            var clusterConfig = ClusterConfiguration(message, false, true, node1, node2);

            var router = new MyRouter(null, null, clusterConfig, sender, null);
            router.Process(message);

            sender.Assert();
        }

        private static IClusterConfiguration ClusterConfiguration(Envelope message, bool local, bool multi, params INodeConfiguration[] nodes)
        {
            var clusterConfig = Mock.Create<IClusterConfiguration>();
            clusterConfig.Arrange(o => o.HasLocalProcessing(message)).Returns(local);
            clusterConfig.Arrange(o => o.IsMultiProcessable(message)).Returns(multi);
            clusterConfig.Arrange(o => o.GetClusterNodesForMessage(message)).Returns(nodes);
            return clusterConfig;
        }

        [Test()]
        public void MultiServerMessageWithLocalProcessingShouldBeSentAnyOtherNodeAnMessageQueue()
        {
            var message = new Envelope(new TestMessage());
            var localNode = Mock.Create<INodeConfiguration>();
            localNode.Arrange(o => o.IsLocal).Returns(true);
            var node1 = Mock.Create<INodeConfiguration>();
            var node2 = Mock.Create<INodeConfiguration>();

            var messageQueue = Mock.Create<IMessageQueue>();
            messageQueue.Arrange(o => o.Add(message)).MustBeCalled();

            var sender = Mock.Create<ISender>();
            sender.Arrange(o => o.Send(message, node1)).MustBeCalled();
            sender.Arrange(o => o.Send(message, node2)).MustBeCalled();

            var clusterConfig = ClusterConfiguration(message, true, true, localNode, node1, node2);

            var router = new MyRouter(null, messageQueue, clusterConfig, sender, null);
            router.Process(message);

            sender.Assert();
            messageQueue.Assert();
        }

        [Test()]
        public void SingleServerMessageWithoutLocalProcessingShouldBeSentViaRoutingStrategy()
        {
            var message = new Envelope(new TestMessage());
            var node1 = Mock.Create<INodeConfiguration>();
            Mock.Arrange(() => node1.Address).Returns(new Uri("http://ShouldNotCall"));

            var node2 = Mock.Create<INodeConfiguration>();
            Mock.Arrange(() => node1.Address).Returns(new Uri("http://ShouldCall"));

            var sender = Mock.Create<ISender>();
            sender.Arrange(o => o.Send(message, node2)).MustBeCalled();

            var clusterConfig = ClusterConfiguration(message, false, false, node1, node2);

            var routingStrategy = Mock.Create<INodeRoutingStrategy>();
            routingStrategy.Arrange(o => o.SelectNode(new[] {node1, node2})).Returns(node2);

            var router = new MyRouter(null, null, clusterConfig, sender, routingStrategy);
            router.Process(message);

            sender.Assert();
        }

        [Test()]
        public void ProcessSyncTest()
        {
            throw new NotImplementedException();
        }
    }
}