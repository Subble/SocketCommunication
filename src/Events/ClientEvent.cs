using SocketCommunication.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocketCommunication.src.Events
{
    public class ClientEvent : IClientEvent
    {
        public ClientEvent()
        {
            Payload = new byte[0];
            RequestData = new byte[0];
        }

        public string ClientID { get; set; }

        public MessageCode EventCode { get; set; }

        public SerializationCode SerializationCode { get; set; }

        public string EventType { get; set; }

        public byte[] Payload { get; set; }

        public byte[] RequestData { get; set; }
    }
}
