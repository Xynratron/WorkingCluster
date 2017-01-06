﻿using System.Linq;
using System.Text;

namespace Esb
{
    public interface IProcessorConfiguration
    {
        void AddOrUpdateProcessingAssembly(byte[] assembly);
        void AddOrUpdateProcessor(IProcessor processor);
        void RemoveProcessingAssembly(byte[] assembly);
        void RemoveProcessor(IProcessor processor);
    }
}
