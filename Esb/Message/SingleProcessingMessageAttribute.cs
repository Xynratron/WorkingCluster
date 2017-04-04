﻿using System;

namespace Esb.Cluster.Messages
{
    /// <summary>
    /// This Message must only be sent to a single node
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    public sealed class SingleProcessingMessageAttribute : Attribute
    {
    }
}