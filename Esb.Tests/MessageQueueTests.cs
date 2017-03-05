using NUnit.Framework;
using Esb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyMessageQueue = Esb.MessageQueue;

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
            throw new NotImplementedException();
        }
    }
}