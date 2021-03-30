using System.Security.Cryptography;
using System.Text;

namespace Liquid.Core.Utils
{
    /// <summary>
    /// Security Hash Extensions.
    /// </summary>
    public static class HashUtils
    {
        /// <summary>
        /// Supported hash algorithms
        /// </summary>
        public enum HashType
        {
            /// <summary>
            /// The HMACMD5 Hash type.
            /// </summary>
            HMacMd5,
            /// <summary>
            /// The HMACSHA1 Hash type.
            /// </summary>
            HMacSha1,
            /// <summary>
            /// The HMACSHA256 Hash type.
            /// </summary>
            HMacSha256,
            /// <summary>
            /// The HMACSHA384 Hash type.
            /// </summary>
            HMacSha384,
            /// <summary>
            /// The HMACSHA512 Hash type.
            /// </summary>
            HMacSha512,
            /// <summary>
            /// The MD5 Hash type.
            /// </summary>
            Md5,
            /// <summary>
            /// The SHA1 Hash type.
            /// </summary>
            Sha1,
            /// <summary>
            /// The SHA256 Hash type.
            /// </summary>
            Sha256,
            /// <summary>
            /// The SHA384 Hash type.
            /// </summary>
            Sha384,
            /// <summary>
            /// The SHA512 Hash type.
            /// </summary>
            Sha512
        }

        /// <summary>
        /// Gets the hash.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="key">The key.</param>
        /// <param name="hash">The hash.</param>
        /// <returns></returns>
        private static byte[] GetComputedHash(string input, string key, HashType hash)
        {
            var inputBytes = Encoding.UTF8.GetBytes(input);
            var inputKey = Encoding.UTF8.GetBytes(key);

            switch (hash)
            {
                case HashType.HMacMd5:
                    return new HMACMD5(inputKey).ComputeHash(inputBytes);
                case HashType.HMacSha1:
                    return new HMACSHA1(inputKey).ComputeHash(inputBytes);
                case HashType.HMacSha256:
                    return new HMACSHA256(inputKey).ComputeHash(inputBytes);
                case HashType.HMacSha384:
                    return new HMACSHA384(inputKey).ComputeHash(inputBytes);
                case HashType.HMacSha512:
                    return new HMACSHA512(inputKey).ComputeHash(inputBytes);
                case HashType.Md5:
                    return MD5.Create().ComputeHash(inputBytes);
                case HashType.Sha1:
                    return SHA1.Create().ComputeHash(inputBytes);
                case HashType.Sha256:
                    return SHA256.Create().ComputeHash(inputBytes);
                case HashType.Sha384:
                    return SHA384.Create().ComputeHash(inputBytes);
                case HashType.Sha512:
                    return SHA512.Create().ComputeHash(inputBytes);
                default:
                    return inputBytes;
            }
        }

        /// <summary>
        /// Computes the hash of the string using a specified hash algorithm
        /// </summary>
        /// <param name="input">The string to hash</param>
        /// <param name="key">The hash key.</param>
        /// <param name="hashType">The hash algorithm to use</param>
        /// <returns>
        /// The resulting hash or an empty string on error
        /// </returns>
        public static string CreateHash(this string input, string key, HashType hashType)
        {
            try
            {
                var hash = GetComputedHash(input, key, hashType);
                var ret = new StringBuilder();

                foreach (var hashByte in hash)
                    ret.Append(hashByte.ToString("x2"));

                return ret.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}