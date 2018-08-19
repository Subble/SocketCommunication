using Subble.Core.Func;
using System.IO;
using System.Threading.Tasks;
using SocketCommunication.Model;
using System;
using Subble.Core.Plugin;

using static Subble.Core.Func.Option;

namespace SocketCommunication.Extensions
{
    internal static class HandshakeExtensions
    {
        /// <summary>
        /// Read 12 bytes from array and try to convert to SemVersion struct
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static async Task<Option<SemVersion>> ReadSemVersion(this Stream stream)
        {
            if (!(await stream.ReadBytesAsync(12)).HasValue(out var array))
                return None<SemVersion>();

            try
            {
                uint major = BitConverter.ToUInt32(array, 0);
                uint minor = BitConverter.ToUInt32(array, 4);
                uint patch = BitConverter.ToUInt32(array, 8);

                return Some(new SemVersion(major, minor, patch));
            }
            catch
            {
                return None<SemVersion>();
            }

        }

        /// <summary>
        /// Read a byte and cast to MessageCode
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static async Task<Option<MessageCode>> ReadMessageCode(this Stream stream)
        {
            return (await stream.ReadByteAsync())
                .Cast(val => (MessageCode)val);
        }


        /// <summary>
        /// Write a SemVersion struct to byte array
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public static async Task WriteSemVersion(this Stream stream, SemVersion version)
        {
            await stream.WriteAsync(BitConverter.GetBytes(version.Major), SizeLength.INT);
            await stream.WriteAsync(BitConverter.GetBytes(version.Minor), SizeLength.INT);
            await stream.WriteAsync(BitConverter.GetBytes(version.Patch), SizeLength.INT);
        }
    }
}
