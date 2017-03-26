using System;
using System.Collections.Generic;

namespace Esb
{
    public class WorkerConfiguration
    {
        public bool IsRootNode { get; set; }
        public Guid NodeId { get; set; }
        public Uri Address { get; set; }
        public List<Uri> RootNodes { get; set; }
    }
}