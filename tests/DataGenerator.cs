#if DEBUG
using SocketCommunication.Extensions;
using SocketCommunication.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using static Subble.Core.Func.Option;

namespace SocketCommunication.tests
{
    internal static class DataGenerator
    {
        public static IEnumerable<object[]> GetListByte()
        {
            var random = new Random();
            for (byte i = 0; i <= 10; i++)
            {
                byte value = (byte)random.Next(byte.MinValue, byte.MaxValue);
                yield return new object[] { value };
            }
        }

        public static IEnumerable<object[]> GetListShort()
        {
            var random = new Random();

            for (byte i = 0; i <= 10; i++)
            {
                short value = (short)random.Next(short.MinValue, short.MaxValue);
                yield return new object[] { value };
            }

        }

        public static IEnumerable<object[]> GetListInt()
        {
            var random = new Random();

            for (byte i = 0; i <= 10; i++)
            {
                int value = random.Next(int.MinValue, int.MaxValue);
                yield return new object[] { value };
            }

        }

        public static IEnumerable<object[]> GetListLong()
        {
            var random = new Random();

            for (byte i = 0; i <= 10; i++)
            {
                long value = LongRandom(long.MinValue, long.MaxValue, random);
                yield return new object[] { value };
            }

        }

        public static IEnumerable<object[]> GetListByteArray(int length)
        {
            var random = new Random();

            for (byte i = 0; i < 10; i++)
            {
                byte[] value = GetRandomByteArray(length, random);
                yield return new object[] { value };
            }
        }

        /// <summary>
        /// Generate byte array with size + content as length
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<object[]> GetSizedByteArray(SizeLength length)
        {
            var rdm = new Random();

            for (byte i = 0; i < 10; i++)
            {
                // generate a random size
                var (size, sizeArray) = GetRandomSize(length, rdm);

                // generate content with size of number
                var contentArray = GetRandomByteArray(size, rdm);

                // calculate total length (size + content)
                var finalSize = size + (int)length;

                // generate result
                var finalArray = new byte[finalSize];
                sizeArray.CopyTo(finalArray, 0);
                contentArray.CopyTo(finalArray, (int)length);

                yield return new object[] { length, finalArray };
            }
        }

        public static IEnumerable<object[]> GetStringByteArray(SizeLength length, string text)
        {
            byte[] size = new byte[0];

            switch (length)
            {
                case SizeLength.BYTE:
                    size = new byte[] { (byte)text.Length };
                    break;
                case SizeLength.SHORT:
                    size = BitConverter.GetBytes((short)text.Length);
                    break;
                case SizeLength.INT:
                    size = BitConverter.GetBytes(text.Length);
                    break;
                case SizeLength.LONG:
                    size = BitConverter.GetBytes((long)text.Length);
                    break;
                default:
                    throw new NotImplementedException();
            }

            byte[] utf8String = Encoding.UTF8.GetBytes(text);

            byte[] result = new byte[size.Length + utf8String.Length];
            size.CopyTo(result, 0);
            utf8String.CopyTo(result, size.Length);

            return new[]
            {
                new object[] { text, length, result }
            };
        }

        public static IEnumerable<object[]> GetStringListArray(SizeLength length, IEnumerable<string> list)
        {
            var value = string.Join(";", list);
            var resultobject = GetStringByteArray(length, value);
            var result = resultobject.First()[2];

            return new[]
            {
                new object[] {list, length, result}
            };
        }

        private static (int size, byte[] sizeBytes) GetRandomSize(SizeLength length, Random rand = null)
        {
            var random = rand ?? new Random();

            int max = length == SizeLength.BYTE ? 255 : short.MaxValue;
            var size = random.Next(1, max);

            return (size, BitConverter.GetBytes(size));
        }

        private static byte[] GetRandomByteArray(long length, Random rdm = null)
        {
            var random = rdm ?? new Random();

            var arr = new byte[length];
            random.NextBytes(arr);
            return arr;
        }

        private static long LongRandom(long min, long max, Random rand)
        {
            byte[] buf = new byte[8];
            rand.NextBytes(buf);
            long longRand = BitConverter.ToInt64(buf, 0);

            return Math.Abs(longRand % (max - min)) + min;
        }

        private static byte[] SizedArrayRandom(SizeLength size, Random rd)
        {
            var length = LongRandom(1, MaxLengthBySize(size), rd);
            var buffer = new byte[length];

            rd.NextBytes(buffer);

            return buffer;
        }

        private static long MaxLengthBySize(SizeLength length)
        {
            switch (length)
            {
                case SizeLength.INT:
                    return int.MaxValue;

                case SizeLength.LONG:
                    return long.MaxValue;

                case SizeLength.SHORT:
                    return short.MaxValue;
            }

            return byte.MaxValue;
        }
    }
}
#endif