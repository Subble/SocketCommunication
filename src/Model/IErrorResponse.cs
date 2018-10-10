namespace SocketCommunication.Model
{
    public interface IErrorResponse
    {
        MessageCode ErrorCode { get; }
        string ErrorMessage { get; }
        byte[] RequestData { get; }
    }
}
