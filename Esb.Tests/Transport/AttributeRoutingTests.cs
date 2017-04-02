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
            throw new NotImplementedException();
        }


        [Test()]
        public void MessageWith_SingleProcessingMessageAttribute_MustBeSentToASingleNode()
        {
            throw new NotImplementedException();
        }

        [Test()]
        public void MessageWith_BroadcastProcessingMessageAttribute_MustBeSentToAllNodes_WithProcessors()
        {
            throw new NotImplementedException();
        }

        [Test()]
        public void MessageWith_BroadcastProcessingMessageAttribute_ShouldNotBeSentToNodes_WithoutProcessor()
        {
            throw new NotImplementedException();
        }
    }
}
