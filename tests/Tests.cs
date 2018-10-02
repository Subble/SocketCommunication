#if DEBUG
using SocketCommunication.Handshake;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SocketCommunication.Tests
{
    public class SocketTests
    {
        [Fact]
        public void Socket_Tests()
        {
            Assert.True(true);
        }

        [Fact]
        public void Socket_Handshake_Request()
        {
            HandshakeRequest reqTarget = new HandshakeRequest
            {
                ClientID = "some client ID",
                Produce = new List<string> { "P1", "P2" },
                RequestCode = Model.MessageCode.Handshake_Access_Request,
                RequestData = new byte[] { 255, 35, 25 },
                Subscribe = new List<string> { "S1", "S2" },
                Version = (1, 0, 20)
            };

            Assert.True(true);
        }
    }
}
#endif