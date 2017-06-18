using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Esb.Tests.Transport
{
    [TestFixture()]
    public class AttributeRoutingTests
    {
        [Test()]
        public void MessageWith_ControllerMessageAttribute_MustBeSentToControllersOnly()
        {
            Assert.Inconclusive();
        }


        [Test()]
        public void MessageWith_SingleProcessingMessageAttribute_MustBeSentToASingleNode()
        {
            Assert.Inconclusive();
        }

        [Test()]
        public void MessageWith_BroadcastProcessingMessageAttribute_MustBeSentToAllNodes_WithProcessors()
        {
            Assert.Inconclusive();
        }

        [Test()]
        public void MessageWith_BroadcastProcessingMessageAttribute_ShouldNotBeSentToNodes_WithoutProcessor()
        {
            Assert.Inconclusive();
        }
    }
}
