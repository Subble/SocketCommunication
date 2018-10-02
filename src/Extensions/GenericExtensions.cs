using Subble.Core.Func;
using System;

using static Subble.Core.Func.Option;

namespace SocketCommunication.Extensions
{
    internal static class GenericExtensions
    {
        /// <summary>
        /// Convert array to long, it must have length 1, 2, 4 or 8
        /// </summary>
        /// <param name="maybeArray"></param>
        /// <returns></returns>
        public static Option<long> ToLong(this Option<byte[]> maybeArray)
        {
            if (!maybeArray.HasValue(out var array))
                return None<long>();

            switch (array.Length)
            {
                case 1:
                    return Some((long)array[0]);
                case 2:
                    return Some((long)BitConverter.ToInt16(array, 0));
                case 4:
                    return Some((long)BitConverter.ToInt32(array, 0));
                case 8:
                    return Some(BitConverter.ToInt64(array, 0));

            }

            return None<long>();
        }
    }
}
