using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using Esb.Cluster;
using Esb.Message;
using Esb.Processing;
using Esb.Tests.Helper;
using Esb.Transport;
using NUnit.Framework;
using Telerik.JustMock;
using Telerik.JustMock.Helpers;

namespace Esb.Tests
{
    [TestFixture()]
    public class IntegrationTest
    {
        [SetUp]
        public void Init()
        {
            _receiversForNodes = new Dictionary<Uri, IReceiver>();
            _sender = Mock.Create<ISender>();
            _sender.Arrange(o => o.Send(Arg.IsAny<Envelope>(), Arg.IsAny<INodeConfiguration>()))
                .DoInstead((Envelope envelope, INodeConfiguration n) =>
                {
                    _receiversForNodes[n.Address].Receive(envelope);
                });
            _workerConfigurationForTest = new Dictionary<Uri, WorkerConfigurationForTest>();
        }

        private Dictionary<Uri, IReceiver> _receiversForNodes;
        private ISender _sender;

        [TearDown]
        public void Cleanup()
        {
            _receiversForNodes = null;
            _sender = null;
            _workerConfigurationForTest = null;
        }

        private IReceiver GetMockedReceiverForAddress(Uri node)
        {
            IReceiver result;
            if (!_receiversForNodes.TryGetValue(node, out result))
            {
                result = Mock.Create<IReceiver>();
                result.Arrange(o => o.Receive(Arg.IsAny<Envelope>())).DoInstead((Envelope envelope) =>
                {
                    result.MessageArrived(envelope);
                });
                _receiversForNodes.Add(node, result);
            }
            return result;
        }

        private IWorker GetWorker(WorkerConfiguration configuration)
        {
            var messageQueue = new MessageQueue();
            var clusterConfig = new ClusterConfiguration();
            var router = new Router(
                GetMockedReceiverForAddress(configuration.Address),
                messageQueue,
                clusterConfig,
                _sender, new SelectRandomNodeRoutingStrategy());
            _workerConfigurationForTest.Add(configuration.Address, new WorkerConfigurationForTest
            {
                ClusterConfig = clusterConfig,
                MessageQueue = messageQueue,
                Router = router,
                WorkerAddress = configuration.Address,
                WorkerConfiguration = configuration
            });
            return new Worker(configuration, clusterConfig, router, messageQueue);
        }

        private Dictionary<Uri, WorkerConfigurationForTest> _workerConfigurationForTest;

        class WorkerConfigurationForTest
        {
            public Uri WorkerAddress { get; set; }
            public MessageQueue MessageQueue { get; set; }
            public Router Router { get; set; }
            public ClusterConfiguration ClusterConfig { get; set; }
            public WorkerConfiguration WorkerConfiguration { get; set; }
        }

        [Test()]
        public void BaseWorkerCommunication()
        {
            var workerUri1 = new Uri("messages://local1");
            var workerUri2 = new Uri("messages://local2");

            var worker1 = GetWorker(new WorkerConfiguration
            {
                Address = workerUri1,
                IsControllerNode = true,
                ControllerNodes = new[] {workerUri1, workerUri2}.ToList()
            });
            
            var worker2 = GetWorker(new WorkerConfiguration
            {
                Address = workerUri2,
                IsControllerNode = true,
                ControllerNodes = new[] { workerUri1, workerUri2 }.ToList()
            });

            worker1.Start();
            worker1.WaitForStartUp();

            worker2.Start();
            worker2.WaitForStartUp();

            _workerConfigurationForTest[workerUri1].ClusterConfig.Nodes.Count().ShouldEqual(2);
            _workerConfigurationForTest[workerUri2].ClusterConfig.Nodes.Count().ShouldEqual(2);

            Console.ReadLine();

            Assert.Inconclusive();
            //    var node = Mock.Create<INodeConfiguration>();
            //    Mock.Arrange(() => node.Address).Returns(_testUri);

            //    var cluster = new ClusterConfiguration();
            //    cluster.AddNode(node);

            //    cluster.Nodes.ShouldContain(node);
        }
    }
}
