using Subble.Core.Func;
using System;
using System.Collections.Generic;
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

        /// <summary>
        /// Compare 2 arrays and match content
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool EqualsContent<T>(this T[] list, T[] target, Func<T, T, bool> matchFunc = null)
        {
            if (list is null || target is null)
                return false;

            if (list.Length != target.Length)
                return false;

            Func<T, T, bool> defMatch = (a, b) => a.Equals(b);
            var match = matchFunc ?? defMatch;

            for (var i = 0; i < list.Length; i++)
                if (!match(list[i], target[i])) { return false; }

            return true;
        }
    }
}
