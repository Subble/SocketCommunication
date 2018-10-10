using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SocketCommunication.Model;
using Subble.Core.Func;
using Subble.Core.Plugin;
using SocketCommunication.Extensions;

using static Subble.Core.Func.Option;

namespace SocketCommunication.Handshake
{
    public class HandshakeRequest : IHandshakeMessage
    {
        public HandshakeRequest()
        {
            RequestData = new byte[0];
        }

        public MessageCode RequestCode { get; set; }

        public SemVersion Version { get; set; }

        public string ClientID { get; set; }

        public IEnumerable<string> Subscribe { get; set; }

        public IEnumerable<string> Produce { get; set; }

        public byte[] RequestData { get; set; }

        public Task<byte[]> ToByteArray()
            => ToByteArray(this);

        public static async Task<byte[]> ToByteArray(IHandshakeMessage m)
        {
            using (var stream = new MemoryStream())
            {

                // Request Code
                await stream.WriteAsync(new[] { (byte)m.RequestCode });

                await stream.WriteSemVersion(m.Version);

                await stream.WriteUTF8Async(m.ClientID, SizeLength.INT);

                const string separator = ";";
                await stream.WriteUTF8Async(separator, SizeLength.INT);

                string subscribeList = string.Join(separator, m.Subscribe);
                string produceList = string.Join(separator, m.Produce);

                await stream.WriteUTF8Async(subscribeList, SizeLength.INT);
                await stream.WriteUTF8Async(produceList, SizeLength.INT);

                await stream.WriteAsync(m.RequestData);

                return stream.ToArray();
            }
        }

        public static Task<Option<HandshakeRequest>> FromByteArray(byte[] source)
        {
            return FromStream(new MemoryStream(source));
        }

        public static async Task<Option<HandshakeRequest>> FromStream(Stream stream)
        {
            var rawCode = await stream.ReadMessageCode();
            if (!rawCode.HasValue(out var messageCode))
                return Empty();

            var rawVersion = await stream.ReadSemVersion();
            if (!rawVersion.HasValue(out var version))
                return Empty();

            var rawClientID = await stream.ReadUnkownStringAsync(SizeLength.INT);
            if (!rawClientID.HasValue(out var clientId))
                return Empty();

            var rawSeparator = await stream.ReadUnkownStringAsync(SizeLength.INT);
            if (!rawSeparator.HasValue(out var separator))
                return Empty();

            var rawSubscribeList = await stream.ReadStringList(SizeLength.INT, separator);
            var rawProduceList = await stream.ReadStringList(SizeLength.INT, separator);

            if (!rawSubscribeList.HasValue(out var subscribers) || !rawProduceList.HasValue(out var producers))
                return Empty();

            var extraData = await stream.ReadToEnd();


            return Some(new HandshakeRequest
            {
                RequestCode = messageCode,
                Version = version,
                ClientID = clientId,
                Subscribe = subscribers,
                Produce = producers,
                RequestData = extraData
            });
        }


        private static Option<HandshakeRequest> Empty() => None<HandshakeRequest>();

        public static bool Equals(IHandshakeMessage x, IHandshakeMessage y)
        {
            return x.ClientID == y.ClientID
                && string.Join(";", x.Produce) == string.Join(";", y.Produce)
                && string.Join(";", x.Subscribe) == string.Join(";", y.Subscribe)
                && x.Version == y.Version
                && x.RequestCode == y.RequestCode
                && x.RequestData.EqualsContent(y.RequestData);
        }

        public override bool Equals(object obj)
        {
            if (obj is IHandshakeMessage message)
                return Equals(this, message);

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }
}
