#if DEBUG
using SocketCommunication.Extensions;
using Xunit;
using System.Threading.Tasks;
using System.IO;
using Subble.Core.Plugin;

namespace SocketCommunication.tests.Extensions
{
    public class HandshakeExtensions
    {
        [Fact]
        public async Task ReadWriteSemVersion()
        {
            byte[] data;
            SemVersion version = (12, 34, 55);

            using (var stream = new MemoryStream(12))
            {
                await stream.WriteSemVersion(version);
                stream.Position = 0;
                Assert.True((await stream.ReadBytesAsync(12)).HasValue(out var result));

                data = result;
                Assert.Equal(12, data.Length);
            }

            using (var stream = new MemoryStream(data))
            {
                (await stream.ReadSemVersion()).HasValue(out var result);
                Assert.Equal(version, result);
            }
        }
    }
}
#endif