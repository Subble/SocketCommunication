#if DEBUG
using System;
using System.Linq;
using System.Collections.Generic;
using SocketCommunication.Extensions;
using SocketCommunication.tests;
using Xunit;

using static Subble.Core.Func.Option;

namespace SocketCommunication.tests.Extensions
{
    public class GenericExtensions
    {
        [Fact]
        public void OptionByteArray_ToLong_IgnoreNone()
        {
            var subject = None<byte[]>().ToLong();
            Assert.False(subject.HasValue());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        [InlineData(9)]
        [InlineData(10)]
        public void OptionByteArray_ToLong_OnlyMatchLength(int length)
        {
            var array = Some(new byte[length]);
            var subject = array.ToLong();

            bool IsValid(int num) => (new[] { 1, 2, 4, 8 }).Contains(num);
            var isValidLength = IsValid(length);

            // If length is valid it shoud have value
            Assert.True(isValidLength == subject.HasValue());
        }

        [Theory]
        [MemberData(nameof(GetListByte))]
        public void OptionArray_ToLong_Convert1Byte(byte input)
        {
            var subject = Some(new byte[] { input }).ToLong();

            Assert.True(subject.HasValue(out var result));
            Assert.Equal(input, result);
        }

        [Theory]
        [MemberData(nameof(GetListShort))]
        public void OptionArray_ToLong_Convert2Byte(short input)
        {
            var subject = Some(BitConverter.GetBytes(input)).ToLong();

            Assert.True(subject.HasValue(out var result));
            Assert.Equal(input, result);
        }

        [Theory]
        [MemberData(nameof(GetListInt))]
        public void OptionArray_ToLong_Convert4Byte(int input)
        {
            var subject = Some(BitConverter.GetBytes(input)).ToLong();

            Assert.True(subject.HasValue(out var result));
            Assert.Equal(input, result);
        }

        [Theory]
        [MemberData(nameof(GetListInt))]
        public void OptionArray_ToLong_Convert8Byte(long input)
        {
            var subject = Some(BitConverter.GetBytes(input)).ToLong();

            Assert.True(subject.HasValue(out var result));
            Assert.Equal(input, result);
        }

        public static IEnumerable<object[]> GetListByte => DataGenerator.GetListByte();
        public static IEnumerable<object[]> GetListInt => DataGenerator.GetListInt();
        public static IEnumerable<object[]> GetListLong => DataGenerator.GetListLong();
        public static IEnumerable<object[]> GetListShort => DataGenerator.GetListShort();
    }
}
#endif