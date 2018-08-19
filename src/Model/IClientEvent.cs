using System;
using System.Collections.Generic;
using System.Text;

namespace SocketCommunication.Model
{
    public interface IClientEvent : IEvent
    {
        string ClientID { get; }
    }
}
