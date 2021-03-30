using System;

namespace Liquid.Core.Utils
{
    /// <summary>
    /// Byte Extensions Class.
    /// </summary>
    public static class ByteUtils
    {
        /// <summary>
        /// Gets the size of the kb.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns></returns>
        public static double GetKbSize(this byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0) return 0;
            return bytes.Length / 1024d;
        }

        /// <summary>
        /// Gets the size of the mb.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns></returns>
        public static double GetMbSize(this byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0) return 0;
            return bytes.Length / Math.Pow(1024, 2);
        }

        /// <summary>
        /// Gets the size of the gb.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns></returns>
        public static double GetGbSize(this byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0) return 0;
            return bytes.Length / Math.Pow(1024, 3);
        }
    }
}