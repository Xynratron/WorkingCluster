using NUnit.Framework;
using System;
using System.Linq;
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
            cluster.Nodes.ShouldNotContain(node);}

        [Test()]
        public void AddProcessorsToNodeTest()
        {
            var node = Mock.Create<INodeConfiguration>();
            Mock.Arrange(() => node.Address).Returns(_testUri);

            var processor = Mock.Create<IProcessor>();

            var cluster = new ClusterConfiguration();
            cluster.AddNode(node);
            cluster.AddProcessorsToNode(node, processor);

            cluster.Nodes.Any(o => o.Processors.Contains(processor)).ShouldBeTrue();
        }

        [Test()]
        public void RemoveProcessorsFromNodeTest()
        {
            var node = Mock.Create<INodeConfiguration>();
            Mock.Arrange(() => node.Address).Returns(_testUri);
            
            var processor = Mock.Create<IProcessor>();

            var cluster = new ClusterConfiguration();
            cluster.AddNode(node);
            cluster.AddProcessorsToNode(node, processor);

            cluster.Nodes.Any(o => o.Processors.Contains(processor)).ShouldBeTrue();

            var node2 = Mock.Create<INodeConfiguration>();
            Mock.Arrange(() => node2.Address).Returns(_testUri);

            cluster.RemoveNode(node2);
            cluster.Nodes.Any(o => o.Processors.Contains(processor)).ShouldBeFalse();
        }

        [Test()]
        public void HasLocalProcessingTest()
        {
            throw new NotImplementedException();
        }

        [Test()]
        public void GetClusterNodesForMessageTest()
        {
            throw new NotImplementedException();
        }

        [Test()]
        public void IsMultiProcessableTest()
        {
            throw new NotImplementedException();
        }
    }
}