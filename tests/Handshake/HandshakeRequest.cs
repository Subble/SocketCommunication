#if DEBUG
using Xunit;
using SocketCommunication.Handshake;
using System.Threading.Tasks;

namespace SocketCommunication.tests.Handshake
{
    public class HandshakeRequestTests
    {
        [Fact]
        public async Task HandshakeRequestFromTo()
        {
            var subject = new HandshakeRequest
            {
                ClientID = "client",
                Produce = new[] { "p1", "p2" },
                Subscribe = new[] { "s1", "s2" },
                RequestCode = Model.MessageCode.Handshake_Access_Request,
                RequestData = new byte[] { 1, 45, 0, 0, 233, 22 },
                Version = (1, 2, 1024)
            };

            var subjectBytes = await subject.ToByteArray();

            Assert.True((await HandshakeRequest.FromByteArray(subjectBytes)).HasValue(out var target));
            Assert.True(subject.Equals(target));
        }
    }
}
#endif