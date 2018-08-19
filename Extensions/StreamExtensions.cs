using Subble.Core.Func;
using System.IO;
using System.Threading.Tasks;
using SocketCommunication.Model;
using System;
using Subble.Core.Plugin;
using System.Text;
using System.Collections.Generic;
using System.Linq;

using static Subble.Core.Func.Option;

namespace SocketCommunication.Extensions
{
    internal static class StreamExtensions
    {
        /// <summary>
        /// Read 1 byte from the stream
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static async Task<Option<byte>> ReadByteAsync(this Stream stream)
        {
            var data = await stream.ReadBytesAsync(1);

            if (data.HasValue(out var value) && value.Length == 1)
                return Some(value[0]);

            return None<byte>();
        }

        /// <summary>
        /// Read from a stream
        /// </summary>
        /// <param name="stream">source stream to read</param>
        /// <param name="length">bytes length to read</param>
        /// <returns>if the length can be read, it has a value</returns>
        public static async Task<Option<byte[]>>  ReadBytesAsync(this Stream stream, int length)
        {
            if (!stream.CanRead)
                return None<byte[]>();

            var buffer = new byte[length];
            var readLength = await stream.ReadAsync(buffer, 0, length);

            if (length != readLength)
                return None<byte[]>();

            return Some(buffer);
        }

        /// <summary>
        /// Read from a stream
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static async Task<Option<byte[]>> ReadBytesAsync(this Stream stream, long length)
        {
            if (!stream.CanRead)
                return None<byte[]>();

            // Use a more effective method
            if (length < int.MaxValue)
                return await stream.ReadBytesAsync((int)length);

            var buffer = new byte[length];
            int offset = 0;

            while (length > 0)
            {
                var bufferSize = length > int.MaxValue ? int.MaxValue : (int)length;
                length -= bufferSize;

                var readLength = await stream.ReadAsync(buffer, offset, bufferSize);

                if (bufferSize != readLength)
                    return None<byte[]>();

                offset += bufferSize;
            }

            return Some(buffer);
        }

        /// <summary>
        /// when you have the length + message to read
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static async Task<Option<byte[]>> ReadUnknonwBytesAsync(this Stream stream, SizeLength length)
        {
            var maybeBytesToRead = await stream.ReadBytesAsync((int)length);

            if (!maybeBytesToRead.ToLong().HasValue(out var bytesToRead))
                return None<byte[]>();

            return await stream.ReadBytesAsync(bytesToRead);
        }

        /// <summary>
        /// read a UTF8 string from a byte array
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static async Task<Option<string>> ReadUnkownStringAsync(this Stream stream, SizeLength length)
        {
            var maybeBytes = await stream.ReadUnknonwBytesAsync(length);
            if (!maybeBytes.HasValue(out var bytes))
                return None<string>();

            return Some(Encoding.UTF8.GetString(bytes));
        }

        /// <summary>
        /// Read a UTF8 string and split it using the separator
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="length"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static async Task<Option<IEnumerable<string>>> ReadStringList(this Stream stream, SizeLength length, string separator)
        {
            var rawString = await stream.ReadUnkownStringAsync(length);
            return rawString
                .Pipe(e => e.Split(separator, StringSplitOptions.RemoveEmptyEntries))
                .Pipe(e => e.AsEnumerable());
        }

        /// <summary>
        /// Read bytes until it reaches the end
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static async Task<byte[]> ReadToEnd(this Stream stream)
        {
            List<byte> list = new List<byte>();

            var bufferSize = 1024;
            var buffer = new byte[bufferSize];
            var readLength = 0;

            do
            {
                readLength = await stream.ReadAsync(buffer, 0, bufferSize);
                list.AddRange(buffer.Take(readLength));
                buffer = new byte[bufferSize];
            } while (readLength == bufferSize);

            return list.ToArray();
        }

        /// <summary>
        /// Write byte array to stream
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static async Task WriteAsync(this Stream stream, byte[] buffer)
        {
            await stream.WriteAsync(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// Write length + byte[]
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="buffer"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static async Task WriteAsync(this Stream stream, byte[] buffer, SizeLength length)
        {
            byte[] bytesLength;
            switch (length)
            {
                case SizeLength.BYTE:
                    bytesLength = BitConverter.GetBytes((byte)buffer.Length);
                    break;

                case SizeLength.SHORT:
                    bytesLength = BitConverter.GetBytes((Int16)buffer.Length);
                    break;

                case SizeLength.INT:
                    bytesLength = BitConverter.GetBytes(buffer.Length);
                    break;

                case SizeLength.LONG:
                    bytesLength = BitConverter.GetBytes(buffer.LongLength);
                    break;

                default:
                    throw new InvalidCastException("Can´t find match for length: " + length.ToString());
            }

            await stream.WriteAsync(bytesLength);
            await stream.WriteAsync(buffer);
        }


        /// <summary>
        /// Write a UTF8 string
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static async Task WriteUTF8Async(this Stream stream, string value, SizeLength length)
        {
            await stream.WriteAsync(Encoding.UTF8.GetBytes(value), length);
        }
    }
}
