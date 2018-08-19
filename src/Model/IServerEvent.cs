using System;
using System.Collections.Generic;
using System.Text;

namespace SocketCommunication.Model
{
    public interface IServerEvent : IEvent
    {
        DateTime DateTime { get; }
        string EventGUID { get; }
        string Source { get; }
    }
}
