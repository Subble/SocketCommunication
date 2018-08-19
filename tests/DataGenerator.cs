#if DEBUG
using System;
using System.Collections.Generic;

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
                byte[] value = new byte[length];
                random.NextBytes(value);
                yield return new object[] { value };
            }
        }

        private static long LongRandom(long min, long max, Random rand)
        {
            byte[] buf = new byte[8];
            rand.NextBytes(buf);
            long longRand = BitConverter.ToInt64(buf, 0);

            return Math.Abs(longRand % (max - min)) + min;
        }
    }
}
#endif