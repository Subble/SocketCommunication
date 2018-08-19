using Subble.Core.Plugin;
using System.Collections.Generic;

namespace SocketCommunication.Model
{
    public interface IHandshakeMessage
    {
        MessageCode RequestCode { get; }
        SemVersion Version { get; }

        string ClientID { get; }
        IEnumerable<string> Subscribe { get; }
        IEnumerable<string> Produce { get; }
        byte[] RequestData { get; }
    }
}
