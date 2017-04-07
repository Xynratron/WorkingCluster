﻿using NUnit.Framework;
using System;
using System.Linq;
using Esb.Message;
using Esb.Tests.Helper;
using Esb.Transport;
using Telerik.JustMock;
using Telerik.JustMock.Helpers;
using MyMessageQueue = Esb.Message.MessageQueue;

namespace Esb.Tests
{
    [TestFixture()]
    public class MessageQueueTests
    {
        private class Message1
        {
        }
        private class Message2
        {
        }
        [Test()]
        public void AddTest()
        {
            var message1 = new Envelope(new Message1(), Priority.Normal);
            var messageQueue = new MyMessageQueue();

            messageQueue.Add(message1);

            messageQueue.Messages.First().ShouldEqual(message1);
        }

        [Test()]
        public void GetNextMessageTest()
        {
            var message1 = new Envelope(new Message1(), Priority.Normal);
            var messageQueue = new MyMessageQueue();

            messageQueue.Add(message1);

            messageQueue.GetNextMessage().ShouldEqual(message1);
        }

        [Test()]
        public void SuspendMessagesTest()
        {
            var message1 = new Envelope(new Message1(), Priority.Normal);
            var message2 = new Envelope(new Message2(), Priority.Normal);
            var messageQueue = new MyMessageQueue();

            messageQueue.Add(message1);
            messageQueue.Add(message2);

            messageQueue.SuspendMessages(typeof(Message1));
            messageQueue.GetNextMessage().ShouldEqual(message2);
        }

        [Test()]
        public void SuspendedMessagIsNotQueued()
        {
            var message1 = new Envelope(new Message1(), Priority.Normal);
            var messageQueue = new MyMessageQueue();

            messageQueue.SuspendMessages(typeof(Message1));
            messageQueue.Add(message1);

            messageQueue.Messages.All(o => o.MessageType != typeof(Message1)).Should(Be.True);
        }

        [Test()]
        public void ResumeMessagesTest()
        {
            var message1 = new Envelope(new Message1(), Priority.High);
            var message2 = new Envelope(new Message2(), Priority.Normal);
            var messageQueue = new MyMessageQueue();

            messageQueue.Add(message1);
            messageQueue.Add(message2);

            messageQueue.SuspendMessages(typeof(Message1));
            messageQueue.Messages.First().ShouldEqual(message2);
            
            messageQueue.ResumeMessages(typeof(Message1));
            messageQueue.Messages.First().ShouldEqual(message1);
        }

        [Test()]
        public void RerouteMessagesTest()
        {
            var router = Mock.Create<IRouter>();
            var message1 = new Envelope(new TestMessage(), Priority.High);
            router.Arrange(o => o.Process(message1)).MustBeCalled();

            var messageQueue = new MyMessageQueue();

            messageQueue.Add(message1);
            messageQueue.RerouteMessages(typeof(TestMessage));

            router.AssertAll();

            Assert.Inconclusive("Router must be assigned to MessageQueue.");
        }

        [Test()]
        public void RemoveMessagesTest()
        {
            var message1 = new Envelope(new TestMessage(), Priority.High);
            
            var messageQueue = new MyMessageQueue();

            messageQueue.Add(message1);
            messageQueue.RemoveMessages(typeof(TestMessage));

            messageQueue.Messages.Any().ShouldBeFalse();
        }
        
        [Test()]
        public void EventOnMessageArivedMustOccurOnMessageAdd()
        {
            var messageQueue = new MyMessageQueue();
            var messageArrivedWasFires = false;
            messageQueue.OnMessageArived += (sender, args) => messageArrivedWasFires = true;

            messageQueue.Add(new Envelope(new TestMessage()));

            messageArrivedWasFires.ShouldBeTrue();
        }

        [Test()]
        public void EventOnMessageArivedShouldNotOccurForSuspendedMessages()
        {
            var messageQueue = new MyMessageQueue();
            var messageArrivedWasFires = false;
            messageQueue.OnMessageArived += (sender, args) => messageArrivedWasFires = true;
            messageQueue.SuspendMessages(typeof(TestMessage));

            messageQueue.Add(new Envelope(new TestMessage()));

            messageArrivedWasFires.ShouldBeFalse();
        }
    }
}