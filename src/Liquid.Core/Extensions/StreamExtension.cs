using System.IO;
using System.Text;

namespace Liquid.Core.Extensions
{
    /// <summary>
    /// Stream extensions class.
    /// </summary>
    public static class StreamExtension
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