using System;
using System.Collections.Generic;
using System.Text;

namespace SocketCommunication.Model
{
    public interface IEvent
    {
        MessageCode EventCode { get; }
        SerializationCode SerializationCode { get; }
        string EventType { get; }
        byte[] Payload { get; }
        byte[] RequestData { get; }
    }
}
