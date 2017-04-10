using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Esb.Cluster;
using Esb.Message;
using Esb.Tests.Helper;
using Esb.Transport;
using NUnit.Framework;
using Telerik.JustMock;

namespace Esb.Tests
{
    [TestFixture()]
    public class IntegrationTest
    {
        [Test()]
        public void AddNodeTest()
        {
        //    var messageQueue = new MessageQueue();

        //    var router = new Router(null, messageQueue, null, null, new SelectRandomNodeRoutingStrategy());


        //    var node = Mock.Create<INodeConfiguration>();
        //    Mock.Arrange(() => node.Address).Returns(_testUri);

        //    var cluster = new ClusterConfiguration();
        //    cluster.AddNode(node);

        //    cluster.Nodes.ShouldContain(node);
        }
    }
}
