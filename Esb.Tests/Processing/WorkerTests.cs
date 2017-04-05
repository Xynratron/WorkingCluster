using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
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

        private Worker GetSingleWorker(WorkerConfiguration workerConfiguration, IRouter router = null, IMessageQueue messageQueue = null)
        {
            if (messageQueue == null)
                messageQueue = Mock.Create<IMessageQueue>();

            if (router == null)
                router = Mock.Create<IRouter>();

            return new Worker(workerConfiguration, router, messageQueue);
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
                 }).WaitForStartUp();
            Assert.NotNull(worker);
        }

        [Test()]
        public void DoubleMasterWorkerAsControllerHaveSameConfiguration()
        {
            var worker1 = GetSingleWorker(new WorkerConfiguration
               {
                   Address = new Uri("http://localhost/1"),
                   ControllerNodes = new List<Uri>(new [] {new Uri("http://localhost/1"), new Uri("http://localhost/2") }),
                   IsControllerNode = true
               }).WaitForStartUp();

            var worker2 = GetSingleWorker(new WorkerConfiguration
            {
                Address = new Uri("http://localhost/2"),
                ControllerNodes = new List<Uri>(new[] { new Uri("http://localhost/1"), new Uri("http://localhost/2") }),
                IsControllerNode = true
            }).WaitForStartUp();
            
            Assert.NotNull(worker1);
            Assert.NotNull(worker2);
            Assert.Inconclusive();
        }

        [Test()]
        public void AddProcessorTest()
        {
            var worker1 = GetSingleWorker(new WorkerConfiguration
            {
                Address = new Uri("http://localhost/1"),
                ControllerNodes = new List<Uri>(new[] { new Uri("http://localhost/1"), new Uri("http://localhost/2") }),
                IsControllerNode = true
            }).WaitForStartUp();
            Assert.Inconclusive();

        }

        [Test()]
        public void StartTest()
        {
            var worker1 = GetSingleWorker(new WorkerConfiguration
            {
                Address = new Uri("http://localhost/1"),
                ControllerNodes = new List<Uri>(new[] { new Uri("http://localhost/1"), new Uri("http://localhost/2") }),
                IsControllerNode = true
            }).WaitForStartUp();
            worker1.Stop();
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
                ControllerNodes = new List<Uri>(new[] { new Uri("http://localhost/1"), new Uri("http://localhost/2") }),
                IsControllerNode = true
            }).WaitForStartUp();
            Assert.AreEqual(WorkerStatus.Started, worker1.Status);

            worker1.Stop();
            Assert.AreEqual(WorkerStatus.Stopped, worker1.Status);
        }



    }
}