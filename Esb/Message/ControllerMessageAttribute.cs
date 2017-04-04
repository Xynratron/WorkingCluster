using System;

namespace Esb.Cluster.Messages
{
    /// <summary>
    /// This Message must be sent to a Controller
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    public sealed class ControllerMessageAttribute : Attribute
    {
    }
}