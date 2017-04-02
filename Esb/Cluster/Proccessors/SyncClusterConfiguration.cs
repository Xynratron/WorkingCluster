﻿using System;
using Esb.Message;
using Esb.Processing;

namespace Esb.Cluster.Proccessors
{
    public class SyncClusterConfiguration : IProcessor<ClusterConfiguration>
    {
        public void Process(IEnvironment environment, Envelope envelope, ClusterConfiguration message)
        {
            throw new NotImplementedException();
        }

        public Type ProcessingType => typeof(ClusterConfiguration);
        public IProcessor<ClusterConfiguration> GetInstance => new SyncClusterConfiguration();
        object IProcessor.GetInstance => GetInstance;
    }
}