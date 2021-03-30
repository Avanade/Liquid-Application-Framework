using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Liquid.Core.Utils
{
    /// <summary>
    /// Xml Extensions Class.
    /// </summary>
    public static  class XmlUtils
    {
        /// <summary>
        /// Serializes an object to xml.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public static string ToXml(this object obj)
        {
            string retVal;
            using (var ms = new MemoryStream())
            {
                var xs = new XmlSerializer(obj.GetType());
                xs.Serialize(ms, obj);
                ms.Flush();
                ms.Position = 0;
                retVal = ms.AsStringUtf8();
            }
            return retVal;
        }

        /// <summary>
        /// Parse a xml to an object.
        /// </summary>
        /// <typeparam name="T">type of object.</typeparam>
        /// <param name="str">The xml string.</param>
        /// <returns></returns>
        public static T ParseXml<T>(this string str) where T : new()
        {
            var xs = new XmlSerializer(typeof(T));
            var stream = str.ToStreamUtf8();
            if (xs.CanDeserialize(new XmlTextReader(stream)))
            {
                stream.Position = 0;
                return (T) xs.Deserialize(stream);
            }
            return default;
        }
    }
}