using System;

namespace Esb
{
    public interface IProcessor
    {
        Type ProcessingType { get; set; }

    }
}