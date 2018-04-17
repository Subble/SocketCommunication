using Subble.Core.Plugin;
using System;

namespace SocketCommunication
{
    public class PluginInfo : IPluginInfo
    {
        public string GUID => "aa20ab12-5dfe-4b37-9c2b-0fa48dcbd1e8";

        public string Name => "SocketCommunication";

        public string Creator => "David Pires";

        public string Repository => "https://github.com/Subble/SocketCommunication";

        public string Support => "https://github.com/Subble/SocketCommunication/issues";

        public string Licence => "MIT";
    }
}
