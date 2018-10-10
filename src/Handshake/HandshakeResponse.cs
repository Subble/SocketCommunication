using SocketCommunication.Extensions;
using SocketCommunication.Model;
using Subble.Core.Func;
using System;
using System.IO;
using System.Threading.Tasks;

using static Subble.Core.Func.Option;

namespace SocketCommunication.Handshake
{
    public class HandshakeResponse : IHandshakeResponse
    {
        public HandshakeResponse()
        {
            RequestData = new byte[0];
        }

        public MessageCode ResponseCode { get; set; }

        public string Message { get; set; }

        public byte[] RequestData { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is IHandshakeResponse r))
                return false;

            return Equals(this, r);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public Task<byte[]> ToByteArrayAsync()
            => ToByteArrayAsync(this);

        public static bool Equals(IHandshakeResponse x, IHandshakeResponse y)
        {
            return x.Message == y.Message
                && x.ResponseCode == y.ResponseCode
                && x.RequestData.EqualsContent(y.RequestData);
        }

        public static async Task<byte[]> ToByteArrayAsync(IHandshakeResponse r)
        {
            using (var stream = new MemoryStream())
            {
                stream.WriteByte((byte)r.ResponseCode);

                await stream.WriteUTF8Async(r.Message, SizeLength.INT);

                await stream.WriteAsync(r.RequestData);

                return stream.ToArray();
            }
        }

        public static Task<Option<HandshakeResponse>> FromByteArrayAsync(byte[] source)
            => FromStreamAsync(new MemoryStream(source));

        public static async Task<Option<HandshakeResponse>> FromStreamAsync(Stream stream)
        {
            if (!(await stream.ReadMessageCode()).HasValue(out var messageCode))
                return Empty();

            if (!(await stream.ReadUnkownStringAsync(SizeLength.INT)).HasValue(out var message))
                return Empty();

            var extraData = await stream.ReadToEnd();

            return Some(new HandshakeResponse
            {
                Message = message,
                RequestData = extraData,
                ResponseCode = messageCode
            });
        }

        public static Option<HandshakeResponse> Empty()
            => None<HandshakeResponse>();
    }
}
