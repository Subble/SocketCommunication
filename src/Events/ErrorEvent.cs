using SocketCommunication.Model;
using SocketCommunication.Extensions;
using System.IO;
using System.Threading.Tasks;
using Subble.Core.Func;

using static Subble.Core.Func.Option;

namespace SocketCommunication.Events
{
    public class ErrorEventResponse : IErrorResponse
    {
        ErrorEventResponse()
        {
            RequestData = new byte[0];
        }

        public MessageCode ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public byte[] RequestData { get; set; }

        public Task<byte[]> ToByteArrayAsync()
            => ToByteArrayAsync(this);

        public override bool Equals(object obj)
        {
            if (!(obj is IErrorResponse r))
                return false;

            return Equals(this, r);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool Equals(IErrorResponse x, IErrorResponse y)
        {
            return x.ErrorCode == y.ErrorCode
                && x.ErrorMessage == y.ErrorMessage
                && x.RequestData.EqualsContent(y.RequestData);
        }

        public static async Task<byte[]> ToByteArrayAsync(IErrorResponse resp)
        {
            using (var stream = new MemoryStream())
            {
                stream.WriteByte((byte)resp.ErrorCode);

                await stream.WriteUTF8Async(resp.ErrorMessage, SizeLength.INT);

                await stream.WriteAsync(resp.RequestData);

                return stream.ToArray();
            }
        }

        public static async Task<Option<ErrorEventResponse>> FromStreamAsync(Stream stream)
        {
            if (!(await stream.ReadMessageCode()).HasValue(out var code))
                return Empty();

            if (!(await stream.ReadUnkownStringAsync(SizeLength.INT)).HasValue(out var message))
                return Empty();

            var data = await stream.ReadToEnd();

            return Some(new ErrorEventResponse
            {
                ErrorCode = code,
                ErrorMessage = message,
                RequestData = data
            });
        }

        public static Task<Option<ErrorEventResponse>> FromByteArrayAsync(byte[] source)
            => FromStreamAsync(new MemoryStream(source));

        public static Option<ErrorEventResponse> Empty()
            => None<ErrorEventResponse>();
    }
}
