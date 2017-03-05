using NUnit.Framework;
using Esb.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telerik.JustMock;
using Telerik.JustMock.Helpers;

namespace Esb.Transport.Tests
{
    [TestFixture()]
    public class RouterTests
    {
        public class TestMessage { }

        [Test()]
        public void SingleServerMessageWithLocalProcessingShouldEndInMessageQueue()
        {
            var message = new Envelope(new TestMessage(), Priority.Normal);

            var messageQueue = Mock.Create<IMessageQueue>();
            messageQueue.Arrange(o => o.Add(message)).MustBeCalled();

            var clusterConfig = Mock.Create<IClusterConfiguration>();
            clusterConfig.Arrange(o => o.HasLocalProcessing(message)).Returns(true);
            clusterConfig.Arrange(o => o.IsMultiProcessable(message)).Returns(false);
            
            var router = new Router(null, messageQueue, clusterConfig, null, null);
            router.Process(message);

            messageQueue.Assert();

        }

        [Test()]
        public void SingleServerMessageWithoutLocalProcessingShouldBeSentToAnotherNode()
        {
            var message = new Envelope(new TestMessage(), Priority.Normal);

            var sender = Mock.Create<ISender>();
            sender.Arrange(o => o.Send(message)).MustBeCalled();
            
            var clusterConfig = Mock.Create<IClusterConfiguration>();
            clusterConfig.Arrange(o => o.HasLocalProcessing(message)).Returns(false);
            clusterConfig.Arrange(o => o.IsMultiProcessable(message)).Returns(false);

            var router = new Router(null, null, clusterConfig, sender, null);
            router.Process(message);

            sender.Assert();
        }

        [Test()]
        public void MultiServerMessageWithoutLocalProcessingShouldBeSentAnyOtherNode()
        {
            var message = new Envelope(new TestMessage(), Priority.Normal);
            var node1 = Mock.Create<INodeConfiguration>();
            var node2 = Mock.Create<INodeConfiguration>();

            var sender = Mock.Create<ISender>();
            sender.Arrange(o => o.Send(message, node1)).MustBeCalled();
            sender.Arrange(o => o.Send(message, node2)).MustBeCalled();

            var clusterConfig = Mock.Create<IClusterConfiguration>();
            clusterConfig.Arrange(o => o.HasLocalProcessing(message)).Returns(false);
            clusterConfig.Arrange(o => o.IsMultiProcessable(message)).Returns(true);
            clusterConfig.Arrange(o => o.GetClusterNodesForMessage(message)).Returns(new [] { node1, node2 });
            
            var router = new Router(null, null, clusterConfig, sender, null);
            router.Process(message);

            sender.Assert();
        }

        [Test()]
        public void MultiServerMessageWithLocalProcessingShouldBeSentAnyOtherNodeAnMessageQueue()
        {
            var message = new Envelope(new TestMessage(), Priority.Normal);
            var localNode = Mock.Create<INodeConfiguration>();
            localNode.Arrange(o => o.IsLocal).Returns(true);
            var node1 = Mock.Create<INodeConfiguration>();
            var node2 = Mock.Create<INodeConfiguration>();

            var messageQueue = Mock.Create<IMessageQueue>();
            messageQueue.Arrange(o => o.Add(message)).MustBeCalled();

            var sender = Mock.Create<ISender>();
            sender.Arrange(o => o.Send(message, node1)).MustBeCalled();
            sender.Arrange(o => o.Send(message, node2)).MustBeCalled();

            var clusterConfig = Mock.Create<IClusterConfiguration>();
            clusterConfig.Arrange(o => o.HasLocalProcessing(message)).Returns(true);
            clusterConfig.Arrange(o => o.IsMultiProcessable(message)).Returns(true);
            clusterConfig.Arrange(o => o.GetClusterNodesForMessage(message)).Returns(new[] { localNode, node1, node2 });

            var router = new Router(null, messageQueue, clusterConfig, sender, null);
            router.Process(message);

            sender.Assert();
            messageQueue.Assert();
        }

        [Test()]
        public void SingleServerMessageWithoutLocalProcessingShouldBeSentViaRoutingStrategy()
        {
            var message = new Envelope(new TestMessage(), Priority.Normal);
            var node1 = Mock.Create<INodeConfiguration>();
            node1.Address = new Uri("http://ShouldNotCall");
            var node2 = Mock.Create<INodeConfiguration>();
            node2.Address = new Uri("http://ShouldCall");

            var sender = Mock.Create<ISender>();
            sender.Arrange(o => o.Send(message, node2)).MustBeCalled();

            var clusterConfig = Mock.Create<IClusterConfiguration>();
            clusterConfig.Arrange(o => o.HasLocalProcessing(message)).Returns(false);
            clusterConfig.Arrange(o => o.IsMultiProcessable(message)).Returns(false);
            clusterConfig.Arrange(o => o.GetClusterNodesForMessage(message)).Returns(new[] { node1, node2 });

            var routingStrategy = Mock.Create<INodeRoutingStrategy>();
            routingStrategy.Arrange(o => o.SelectNode(new[] {node1, node2})).Returns(node2);

            var router = new Router(null, null, clusterConfig, sender, routingStrategy);
            router.Process(message);

            sender.Assert();
        }
    }
}