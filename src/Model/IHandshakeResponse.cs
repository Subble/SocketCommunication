
namespace SocketCommunication.Model
{
    public interface IHandshakeResponse
    {
        MessageCode ResponseCode { get; }
        string Message { get; }
        byte[] RequestData { get; }
    }
}
