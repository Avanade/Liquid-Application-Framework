using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Liquid.Core.Utils
{
    /// <summary>
    /// Compression and Decompression library.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class ZipUtils
    {
        #region Gzip

        /// <summary>
        /// Compress a string using Gzip format and UTF8 encoding.
        /// </summary>
        /// <param name="inputString">The input string.</param>
        /// <returns>The compressed string.</returns>
        public static byte[] GzipCompress(this string inputString)
        {
            return inputString.GzipCompress(Encoding.UTF8);
        }

        /// <summary>
        /// Compress a string using Gzip format.
        /// </summary>
        /// <param name="inputString">The input string.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>
        /// The compressed string.
        /// </returns>
        public static byte[] GzipCompress(this string inputString, Encoding encoding)
        {
            var inputBytes = encoding.GetBytes(inputString);
            byte[] output;

            using (var outputStream = new MemoryStream())
            {
                using (var gZipStream = new GZipStream(outputStream, CompressionMode.Compress))
                {
                    gZipStream.Write(inputBytes, 0, inputBytes.Length);
                }
                output = outputStream.ToArray();
            }

            return output;
        }

        /// <summary>
        /// Decompress a compressed string using Gzip format and UTF8 encoding.
        /// </summary>
        /// <param name="inputBytes">The compressed bytes.</param>
        /// <returns>The decompressed string.</returns>
        public static string GzipDecompress(this byte[] inputBytes)
        {
            return inputBytes.GzipDecompress(Encoding.UTF8);
        }

        /// <summary>
        /// Decompress a compressed string using Gzip format.
        /// </summary>
        /// <param name="inputBytes">The compressed bytes.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>
        /// The decompressed string.
        /// </returns>
        public static string GzipDecompress(this byte[] inputBytes, Encoding encoding)
        {
            string decompressed;
            using (var inputStream = new MemoryStream(inputBytes))
            using (var gZipStream = new GZipStream(inputStream, CompressionMode.Decompress))
            using (var outputStream = new MemoryStream())
            {
                gZipStream.CopyTo(outputStream);
                var outputBytes = outputStream.ToArray();

                decompressed = encoding.GetString(outputBytes);
            }

            return decompressed;
        }

        #endregion

        #region Deflate

        /// <summary>
        /// Compress a string using Deflate format.
        /// </summary>
        /// <param name="inputString">The input string.</param>
        /// <returns>The compressed string.</returns>
        public static byte[] DeflateCompress(this string inputString)
        {
            return inputString.DeflateCompress(Encoding.UTF8);
        }

        /// <summary>
        /// Compress a string using Deflate format.
        /// </summary>
        /// <param name="inputString">The input string.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>
        /// The compressed string.
        /// </returns>
        public static byte[] DeflateCompress(this string inputString, Encoding encoding)
        {
            var inputBytes = encoding.GetBytes(inputString);
            byte[] output;

            using (var outputStream = new MemoryStream())
            {
                using (var deflateStream = new DeflateStream(outputStream, CompressionMode.Compress))
                {
                    deflateStream.Write(inputBytes, 0, inputBytes.Length);
                }
                output = outputStream.ToArray();
            }

            return output;
        }

        /// <summary>
        /// Decompress a compressed string using Deflate format.
        /// </summary>
        /// <param name="inputBytes">The compressed bytes.</param>
        /// <returns>The decompressed string.</returns>
        public static string DeflateDecompress(this byte[] inputBytes)
        {
            return inputBytes.DeflateDecompress(Encoding.UTF8);
        }

        /// <summary>
        /// Decompress a compressed string using Deflate format.
        /// </summary>
        /// <param name="inputBytes">The compressed bytes.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>
        /// The decompressed string.
        /// </returns>
        public static string DeflateDecompress(this byte[] inputBytes, Encoding encoding)
        {
            string decompressed;
            using (var inputStream = new MemoryStream(inputBytes))
            using (var deflateStream = new DeflateStream(inputStream, CompressionMode.Decompress))
            using (var outputStream = new MemoryStream())
            {
                deflateStream.CopyTo(outputStream);
                var outputBytes = outputStream.ToArray();

                decompressed = encoding.GetString(outputBytes);
            }

            return decompressed;
        }

        #endregion
    }
}