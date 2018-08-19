#if DEBUG
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Subble.Core.Func;
using SocketCommunication.Extensions;
using Xunit;

using static Subble.Core.Func.Option;

namespace SocketCommunication.tests.Extensions
{
    public class StreamExtensions
    {
        [Theory]
        [MemberData(nameof(GetListByte))]
        public async Task ReadByteAsync_Read1Byte(byte input)
        {
            using (var stream = new MemoryStream(new[] { input }))
            {
                var subject = await stream.ReadByteAsync();

                Assert.True(subject.HasValue(out var result));
                Assert.Equal(input, result);
            }
        }

        [Fact]
        public async Task ReadByteAsync_NoneOnEmpty()
        {
            using (var stream = new MemoryStream(new byte[0]))
            {
                var subject = await stream.ReadByteAsync();

                Assert.False(subject.HasValue());
            }
        }

        [Theory]
        [MemberData(nameof(GetListByteArray5))]
        public async Task ReadBytesAsync_Read5BytesInt(byte[] input)
        {
            using (var stream = new MemoryStream(input))
            {
                var subject = await stream.ReadBytesAsync(5);

                Assert.True(subject.HasValue(out var result));
                Assert.Equal(input, result);
            }
        }

        [Theory]
        [MemberData(nameof(GetListByteArray5))]
        public async Task ReadBytesAsync_NoneOnInvalidLength(byte[] input)
        {
            using (var stream = new MemoryStream(input))
            {
                var subject = await stream.ReadBytesAsync(6);

                // When trying to read 6 bytes from an stream with 5 bytes it should return NONE
                Assert.False(subject.HasValue());
            }
        }



        public static IEnumerable<object[]> GetListByte => DataGenerator.GetListByte();
        public static IEnumerable<object[]> GetListByteArray5 => DataGenerator.GetListByteArray(5);
    }
}
#endif