#if DEBUG
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using SocketCommunication.Extensions;
using SocketCommunication.Model;
using Xunit;

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

        [Theory]
        [MemberData(nameof(GetUnkownSizeArray), SizeLength.BYTE)]
        [MemberData(nameof(GetUnkownSizeArray), SizeLength.SHORT)]
        [MemberData(nameof(GetUnkownSizeArray), SizeLength.INT)]
        [MemberData(nameof(GetUnkownSizeArray), SizeLength.LONG)]
        public async Task ReadUnknonwBytesAsync(SizeLength length, byte[] input)
        {
            try
            {
                var expected = new byte[input.Length - (int)length];
                input.Skip((int)length).ToArray().CopyTo(expected, 0);

                using (var stream = new MemoryStream(input))
                {
                    var subject = await stream.ReadUnknonwBytesAsync(length);

                    Assert.True(subject.HasValue(out var result));
                    Assert.True(CompareIEnumerable(expected, result));
                }
            }
            catch (Exception ex)
            {
                Assert.True(false, $"{length} message: {ex.Message}");
            }
        }

        [Theory]
        [MemberData(nameof(GetUnkownSizeString), SizeLength.BYTE)]
        [MemberData(nameof(GetUnkownSizeString), SizeLength.SHORT)]
        [MemberData(nameof(GetUnkownSizeString), SizeLength.INT)]
        [MemberData(nameof(GetUnkownSizeString), SizeLength.LONG)]
        public async Task ReadUnknownStringAsync(string source, SizeLength length, byte[] input)
        {
            using (var stream = new MemoryStream(input))
            {
                var subject = await stream.ReadUnkownStringAsync(length);

                Assert.True(subject.HasValue(out var result));
                Assert.Equal(source, result);
            }
        }

        [Theory]
        [MemberData(nameof(GetUnknownListString), SizeLength.BYTE)]
        [MemberData(nameof(GetUnknownListString), SizeLength.SHORT)]
        [MemberData(nameof(GetUnknownListString), SizeLength.INT)]
        [MemberData(nameof(GetUnknownListString), SizeLength.LONG)]
        public async Task ReadUnkownStringListAsync(IEnumerable<string> source, SizeLength length, byte[] input)
        {
            using (var stream = new MemoryStream(input))
            {
                var subject = await stream.ReadStringList(length, ";");

                Assert.True(subject.HasValue(out var result));
                Assert.True(CompareIEnumerable(source, result));
            }
        }

        private static bool CompareIEnumerable<T>(IEnumerable<T> l1, IEnumerable<T> l2, Func<T, T, bool> compareFunc = null)
        {
            var f1 = l1.ToList();
            var f2 = l2.ToList();

            if (f1.Count != f2.Count)
                return false;

            Func<T, T, bool> defCOmpare = ((a, b) => a.Equals(b));
            Func<T, T, bool> comp = compareFunc ?? defCOmpare;

            foreach (var i1 in f1)
            {
                var match = false;

                foreach (var i2 in f2)
                {
                    if (i1.Equals(i2))
                    {
                        match = true;
                        break;
                    }
                }

                if (!match) return false;
            }

            return true;
        }

        public static IEnumerable<object[]> GetListByte => DataGenerator.GetListByte();
        public static IEnumerable<object[]> GetListByteArray5 => DataGenerator.GetListByteArray(5);

        public static IEnumerable<object[]> GetUnkownSizeArray(SizeLength size)
            => DataGenerator.GetSizedByteArray(size);

        public static IEnumerable<object[]> GetUnkownSizeString(SizeLength size)
            => DataGenerator.GetStringByteArray(size, "some string to show");

        public static IEnumerable<object[]> GetUnknownListString(SizeLength size)
            => DataGenerator.GetStringListArray(size, new string[] { "32321", "dsadsa", "321", "dajyujuy", "fapo" });
    }
}
#endif