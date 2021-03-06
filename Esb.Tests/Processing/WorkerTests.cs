﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Esb.Cluster;
using Esb.Message;
using NUnit.Framework;
using Telerik.JustMock;
using Telerik.JustMock.Helpers;
using Esb.Processing;
using Esb.Tests.Helper;
using Esb.Transport;


namespace Esb.Tests.Processing
{
    [TestFixture()]
    public class WorkerTests
    {

        private Worker GetSingleWorker(WorkerConfiguration workerConfiguration, IRouter router = null,
            IMessageQueue messageQueue = null)
        {
            if (messageQueue == null)
            {
                messageQueue = Mock.Create<IMessageQueue>();
                messageQueue.Arrange<IMessageQueue, Envelope>(o => o.GetNextMessage()).Returns<Envelope>((Envelope)null);
            }
            if (router == null)
                router = Mock.Create<IRouter>();

            return new Worker(workerConfiguration, new ClusterConfiguration(),  router, messageQueue);
        }

        [Test()]
        public void SingleWorkerAsControllerIsStartingUp()
        {
            var worker =
                GetSingleWorker(new WorkerConfiguration
                {
                    Address = new Uri("http://localhost"),
                    ControllerNodes = new List<Uri>(),
                    IsControllerNode = true
                });
            worker.Start();
            worker.WaitForStartUp();
            Assert.NotNull(worker);
        }
        
        [Test()]
        public void AddProcessorTest()
        {
            var worker1 = GetSingleWorker(new WorkerConfiguration
            {
                Address = new Uri("http://localhost/1"),
                ControllerNodes = new List<Uri>(new[] {new Uri("http://localhost/1"), new Uri("http://localhost/2")}),
                IsControllerNode = true
            });
            
            worker1.WaitForStartUp();
        }

        [Test()]
        public void StartTest()
        {
            var worker1 = GetSingleWorker(new WorkerConfiguration
            {
                Address = new Uri("http://localhost/1"),
                ControllerNodes = new List<Uri>(new[] {new Uri("http://localhost/1"), new Uri("http://localhost/2")}),
                IsControllerNode = true
            });
            Assert.AreEqual(WorkerStatus.Stopped, worker1.Status);

            worker1.Start();
            Assert.AreEqual(WorkerStatus.Started, worker1.Status);
        }

        [Test()]
        public void StopTest()
        {
            var worker1 = GetSingleWorker(new WorkerConfiguration
            {
                Address = new Uri("http://localhost/1"),
                ControllerNodes = new List<Uri>(new[] {new Uri("http://localhost/1"), new Uri("http://localhost/2")}),
                IsControllerNode = true
            }).WaitForStartUp();
            Assert.AreEqual(WorkerStatus.Started, worker1.Status);

            worker1.Stop();
            Assert.AreEqual(WorkerStatus.Stopped, worker1.Status);
        }

        [Test()]
        [Ignore("must be redesigned")]
        public void MessagesMustBe_Suspended_Rerouted_IfProcessorIsRemoved_AndMessageIsSingleProcessing()
        {
            var messageQueue = Mock.Create<IMessageQueue>();

            messageQueue.Arrange(o => o.RerouteMessages(typeof(SingleProcessingTestMessage))).MustBeCalled();
            messageQueue.Arrange(o => o.SuspendMessages(typeof(SingleProcessingTestMessage))).MustBeCalled();
            messageQueue.Arrange(o => o.ResumeMessages(typeof(SingleProcessingTestMessage))).OccursNever();
            messageQueue.Arrange(o => o.GetNextMessage()).Returns((Envelope)null);

            var worker1 = GetSingleWorker(GetWorkerConfiguration(), null, messageQueue).WaitForStartUp();
            
            var proc = new SingleProcessingTestMessageProcessor();

            worker1.AddProcessor(proc);

            worker1.RemoveProcessor(proc);

            messageQueue.AssertAll();
            Assert.Inconclusive();
        }

        [Test()]
        [Ignore("must be redesigned")]
        public void MessagesMustBe_Suspended_Removed_IfProcessorIsRemoved_AndMessageIsBroadcastProcessing()
        {
            var messageQueue = Mock.Create<IMessageQueue>();

            messageQueue.Arrange(o => o.RerouteMessages(typeof(BroadcastTestMessage))).MustBeCalled();
            messageQueue.Arrange(o => o.SuspendMessages(typeof(BroadcastTestMessage))).OccursNever();
            messageQueue.Arrange(o => o.ResumeMessages(typeof(BroadcastTestMessage))).OccursNever();
            messageQueue.Arrange(o => o.RemoveMessages(typeof(BroadcastTestMessage))).MustBeCalled();
            messageQueue.Arrange(o => o.GetNextMessage()).Returns((Envelope) null);

            var worker1 = GetSingleWorker(GetWorkerConfiguration(), null, messageQueue).WaitForStartUp();

            var proc = new BroadcastTestMessageProcessor();

            worker1.AddProcessor(proc);

            worker1.RemoveProcessor(proc);

            messageQueue.AssertAll();
            Assert.Inconclusive();
        }


        

        private WorkerConfiguration GetWorkerConfiguration()
        {
            return new WorkerConfiguration
            {
                Address = new Uri("http://localhost/1"),
                ControllerNodes = new List<Uri>(new[] {new Uri("http://localhost/1"), new Uri("http://localhost/2")}),
                IsControllerNode = true
            };
        }
    }
}