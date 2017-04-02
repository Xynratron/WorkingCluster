using System;
using System.Collections.Generic;

namespace Esb.Processing
{
    public class WorkerConfiguration
    {
        public bool IsControllerNode { get; set; }
        public Uri Address { get; set; }
        public List<Uri> ControllerNodes { get; set; }
    }
}