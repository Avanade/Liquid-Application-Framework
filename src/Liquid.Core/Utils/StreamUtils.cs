using System.IO;
using System.Text;

namespace Liquid.Core.Utils
{
    /// <summary>
    /// Stream extensions class.
    /// </summary>
    public static class StreamUtils
    {
        /// <summary>
        /// Gets the string from UTF8 stream.
        /// </summary>
        /// <param name="source">The stream source.</param>
        /// <returns>The result string from stream.</returns>
        public static string AsStringUtf8(this Stream source)
        {
            if (source == null) return null;
            string documentContents;
            using (var readStream = new StreamReader(source, Encoding.UTF8))
            {
                documentContents = readStream.ReadToEnd();
            }

            return documentContents;
        }

        /// <summary>
        /// This method validate if arrayByte is UTF8.
        /// </summary>
        /// <param name="receivedArrayByte">byte[] Received</param>
        /// <returns>true => UTF8</returns>
        public static bool IsUTF8(this byte[] receivedArrayByte)
        {

            int expectedSize = 0;

            for (int i = 0; i < receivedArrayByte.Length; i++)
            {
                if ((receivedArrayByte[i] & 0b10000000) == 0b00000000)
                {
                    expectedSize = 1;
                }
                else if ((receivedArrayByte[i] & 0b11100000) == 0b11000000)
                {
                    expectedSize = 2;
                }
                else if ((receivedArrayByte[i] & 0b11110000) == 0b11100000)
                {
                    expectedSize = 3;
                }
                else if ((receivedArrayByte[i] & 0b11111000) == 0b11110000)
                {
                    expectedSize = 4;
                }
                else if ((receivedArrayByte[i] & 0b11111100) == 0b11111000)
                {
                    expectedSize = 5;
                }
                else if ((receivedArrayByte[i] & 0b11111110) == 0b11111100)
                {
                    expectedSize = 6;
                }
                else
                {
                    return false;
                }

                while (--expectedSize > 0)
                {
                    if (++i >= receivedArrayByte.Length)
                    {
                        return false;
                    }
                    if ((receivedArrayByte[i] & 0b11000000) != 0b10000000)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Gets the string from ASCII stream.
        /// </summary>
        /// <param name="source">The stream source.</param>
        /// <returns>The result string from stream.</returns>
        public static string AsStringAscii(this Stream source)
        {
            if (source == null) return null;
            string documentContents;
            using (var readStream = new StreamReader(source, Encoding.ASCII))
            {
                documentContents = readStream.ReadToEnd();
            }

            return documentContents;
        }

        /// <summary>
        /// Gets the string from UNICODE stream.
        /// </summary>
        /// <param name="source">The stream source.</param>
        /// <returns>The result string from stream.</returns>
        public static string AsStringUnicode(this Stream source)
        {
            if (source == null) return null;
            string documentContents;
            using (var readStream = new StreamReader(source, Encoding.Unicode))
            {
                documentContents = readStream.ReadToEnd();
            }
            return documentContents;
        }

        /// <summary>
        /// Gets the stream from string using UTF8 encoding.
        /// </summary>
        /// <param name="source">The string source.</param>
        /// <returns>UTF8 encoded stream.</returns>
        public static Stream ToStreamUtf8(this string source)
        {
            if (source == null) return null;
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(source));
            return stream;
        }

        /// <summary>
        /// Gets the stream from string using ASCII encoding.
        /// </summary>
        /// <param name="source">The string source.</param>
        /// <returns>ASCII encoded stream.</returns>
        public static Stream ToStreamAscii(this string source)
        {
            if (source == null) return null;
            var stream = new MemoryStream(Encoding.ASCII.GetBytes(source));
            return stream;
        }

        /// <summary>
        /// Gets the stream from string using UTF8 encoding.
        /// </summary>
        /// <param name="source">The string source.</param>
        /// <returns>Unicode encoded stream.</returns>
        public static Stream ToStreamUnicode(this string source)
        {
            if (source == null) return null;
            var stream = new MemoryStream(Encoding.Unicode.GetBytes(source));
            return stream;
        }

        /// <summary>
        /// Converts a stream to byte array.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public static byte[] ToByteArray(this Stream stream)
        {
            using var ms = new MemoryStream();
            stream.CopyTo(ms);
            return ms.ToArray();
        }
    }
}