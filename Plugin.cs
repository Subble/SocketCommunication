﻿using System;
using System.Collections.Generic;
using Subble.Core;
using Subble.Core.Config;
using Subble.Core.Logger;
using Subble.Core.Plugin;

namespace SocketCommunication
{
    public class Plugin : ISubblePlugin
    {
        public IPluginInfo Info
            => new PluginInfo();

        public SemVersion Version
            => new SemVersion(0, 0, 1);

        public long LoadPriority => 10;

        public IEnumerable<Dependency> Dependencies
            => new[]
            {
                new Dependency(typeof(ILogger), 0, 0, 1),
                new Dependency(typeof(IConfigManager), 0, 0, 1)
            };


        public bool Initialize(ISubbleHost host)
        {
            if (host is null)
                return false;
                
            return true;
        }
    }
}
