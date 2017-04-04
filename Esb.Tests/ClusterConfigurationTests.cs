using NUnit.Framework;
using System;
using System.Linq;
using Esb.Cluster;
using Esb.Message;
using Esb.Processing;
using Esb.Tests.Helper;
using Telerik.JustMock;
using Telerik.JustMock.Helpers;

namespace Esb.Tests
{
    [TestFixture()]
    public class ClusterConfigurationTests
    {
        private static Uri _testUri = new Uri("tcp://dns");

      

        [Test()]
        public void AddNodeTest()
        {
            var node = Mock.Create<INodeConfiguration>();
            Mock.Arrange(() => node.Address).Returns(_testUri);

            var cluster = new ClusterConfiguration();
            cluster.AddNode(node);

            cluster.Nodes.ShouldContain(node);
        }

        [Test()]
        public void AddNodeHasNoDublicates()
        {
            var cluster = new ClusterConfiguration();

            var node1 = Mock.Create<INodeConfiguration>();
            Mock.Arrange(() => node1.Address).Returns(_testUri);
            cluster.AddNode(node1);

            var node2 = Mock.Create<INodeConfiguration>();
            Mock.Arrange(() => node2.Address).Returns(_testUri);
            cluster.AddNode(node2);
            
            (cluster.Nodes.Count(o => o.Address == _testUri) > 1).ShouldBeFalse();
        }

        [Test()]
        public void RemoveNodeTest()
        {
            var node = Mock.Create<INodeConfiguration>();
            var cluster = new ClusterConfiguration();

            Mock.Arrange(() => node.Address).Returns(_testUri);

            cluster.AddNode(node);
            cluster.Nodes.ShouldContain(node);

            cluster.RemoveNode(node);
            cluster.Nodes.ShouldNotContain(node);
        }

        [Test()]
        public void AddProcessorsToNodeWithInstanceTest()
        {
            var node = Mock.Create<INodeConfiguration>();
            Mock.Arrange(() => node.Address).Returns(_testUri);

            var processor = new TestMessageProcessor();
            var cluster = new ClusterConfiguration();
            cluster.AddNode(node);
            cluster.AddProcessorsToNode(node, processor);

            cluster.Nodes.Any(o => o.Processors.Any(p => p == processor)).ShouldBeTrue();
        }

        [Test()]
        public void AddProcessorsToNodeWithTypeTest()
        {
            var node = Mock.Create<INodeConfiguration>();
            Mock.Arrange(() => node.Address).Returns(_testUri);

            var processor = new TestMessageProcessor();
            var cluster = new ClusterConfiguration();
            cluster.AddNode(node);
            cluster.AddProcessorsToNode(node, processor.GetType());

            cluster.Nodes.Any(o => o.Processors.Any(p => p == processor)).ShouldBeFalse();
            cluster.Nodes.Any(o => o.Processors.Any(p => p.GetType() == processor.GetType())).ShouldBeTrue();
        }

        [Test()]
        public void RemoveProcessorsFromNodeWithInstanceTest()
        {
            var node = Mock.Create<INodeConfiguration>();
            Mock.Arrange(() => node.Address).Returns(_testUri);

            var processor = new TestMessageProcessor();
            var cluster = new ClusterConfiguration();
            cluster.AddNode(node);
            cluster.AddProcessorsToNode(node, processor);

            cluster.Nodes.Any(o => o.Processors.Any(p => p == processor)).ShouldBeTrue();

            var node2 = Mock.Create<INodeConfiguration>();
            Mock.Arrange(() => node2.Address).Returns(_testUri);

            cluster.RemoveNode(node2);
            cluster.Nodes.Any(o => o.Processors.Any(p => p == processor)).ShouldBeFalse();
        }

        [Test()]
        public void RemoveProcessorsFromNodeWithTypeTest()
        {
            var node = Mock.Create<INodeConfiguration>();
            Mock.Arrange(() => node.Address).Returns(_testUri);

            var processor = new TestMessageProcessor();
            var cluster = new ClusterConfiguration();
            cluster.AddNode(node);
            cluster.AddProcessorsToNode(node, typeof(TestMessageProcessor));

            cluster.Nodes.Any(o => o.Processors.Any(p => p == processor)).ShouldBeFalse();
            cluster.Nodes.Any(o => o.Processors.Any(p => p.GetType() == processor.GetType())).ShouldBeTrue();

            var node2 = Mock.Create<INodeConfiguration>();
            Mock.Arrange(() => node2.Address).Returns(_testUri);

            cluster.RemoveNode(node2);

            cluster.Nodes.Any(o => o.Processors.Any(p => p == processor)).ShouldBeFalse();
            cluster.Nodes.Any(o => o.Processors.Any(p => p.GetType() == processor.GetType())).ShouldBeFalse();
        }

        [Test()]
        public void HasLocalProcessingTest()
        {
            var node = Mock.Create<INodeConfiguration>();
            Mock.Arrange(() => node.Address).Returns(_testUri);
            Mock.Arrange(() => node.IsLocal).Returns(true);

            var cluster = new ClusterConfiguration();
            cluster.AddNode(node);
            cluster.AddProcessorsToNode(node, new TestMessageProcessor());

            cluster.HasLocalProcessing(new Envelope(new TestMessage(), Priority.Normal)).ShouldBeTrue();
            cluster.HasLocalProcessing(new Envelope(new BroadcastTestMessage(), Priority.Normal)).ShouldBeFalse();
        }

        [Test()]
        public void GetClusterNodesForMessageTest()
        {
            throw new NotImplementedException();
        }

        [Test()]
        public void BroadcastMessagesShouldBeMultiprocessable()
        {
            var node = Mock.Create<INodeConfiguration>();
            Mock.Arrange(() => node.Address).Returns(_testUri);

            var cluster = new ClusterConfiguration();
            var broadcastMessage = new Envelope(new BroadcastTestMessage(), Priority.Normal);
            var singleProcessingMessage = new Envelope(new SingleProcessingTestMessage(), Priority.Normal);
            var messageWithoutAttribute = new Envelope(new TestMessage(), Priority.Normal);

            cluster.IsMultiProcessable(broadcastMessage).ShouldBeTrue();
            cluster.IsMultiProcessable(singleProcessingMessage).ShouldBeFalse();
            cluster.IsMultiProcessable(messageWithoutAttribute).ShouldBeFalse();
        }
    }
}