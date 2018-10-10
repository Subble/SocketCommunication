namespace SocketCommunication.Model
{
    public enum MessageCode
    {
        Handshake_Access_Request = 1,
        Handshake_OK_Client_Accept = 2,
        Handshake_OK_Client_Created = 3,

        Handshake_Error_IP_Forbidden = 10,
        Handshake_Error_New_Client_Forbidden = 11,
        Handshake_Error_Client_Invalid = 12,
        Handshake_Error_Unkown = 25,

        Event_Server = 26, // Event created by server
        Event_Client = 27, // Event created by client

        Event_Forbidden = 51,
        Event_Error_Unkown = 52
    }
}
