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
using Esb.Transport;


namespace Esb.Tests.Processing
{
    [TestFixture()]
    public class WorkerTests
    {
        [Test()]
        public void WorkerTest()
        {
            var worker =
                GetSingleWorker(new WorkerConfiguration
                {
                    Address = new Uri("ws://local}"),
                    ControllerNodes = new List<Uri>(),
                    IsControllerNode = true
                });
            WaitForStartUp(worker);
            Assert.NotNull(worker);
        }

        private void WaitForStartUp(Worker worker)
        {
            var sw = new Stopwatch();
            while (worker.Status != WorkerStatus.Started)
            {
                if (sw.ElapsedMilliseconds > 30000)
                    throw new TimeoutException("Waited for 30 seconds but worker did not come up.");
                System.Threading.Thread.Sleep(1);
            }
        }

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
            throw new NotImplementedException();
        }
        [Test()]
        public void DoubleMasterWorkerAsControllerHaveSameConfiguration()
        {
            throw new NotImplementedException();
        }

        [Test()]
        public void InitialStartUpAyncTest()
        {
            throw new NotImplementedException();
        }

        [Test()]
        public void StartTest()
        {
            throw new NotImplementedException();
        }

        [Test()]
        public void StopTest()
        {
            throw new NotImplementedException();
        }
    }
}