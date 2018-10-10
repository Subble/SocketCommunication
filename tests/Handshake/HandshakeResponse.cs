#if DEBUG
using SocketCommunication.Handshake;
using System.Threading.Tasks;
using Xunit;

namespace SocketCommunication.tests.Handshake
{
    public class HandshakeResponseTest
    {
        [Fact]
        public async Task HandshakeResponseFromTo()
        {
            var subject = new HandshakeResponse
            {
                Message = "Success request",
                RequestData = new byte[] { 33, 134, 0, 0, 0, 122 },
                ResponseCode = Model.MessageCode.Handshake_OK_Client_Accept
            };

            var subjectBytes = await subject.ToByteArrayAsync();

            Assert.True(
                    (await HandshakeResponse.FromByteArrayAsync(subjectBytes))
                    .HasValue(out var target)
                );
            Assert.True(subject.Equals(target));
        }
    }
}
#endif