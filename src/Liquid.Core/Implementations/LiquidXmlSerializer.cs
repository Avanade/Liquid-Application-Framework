using Liquid.Core.Exceptions;
using Liquid.Core.Interfaces;
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Liquid.Core.Implementations
{
    /// <summary>
    /// Implementation of Liquid Serializer to XML.
    /// </summary>
    public class LiquidXmlSerializer : ILiquidSerializer
    {
        /// <summary>
        /// Serializes object to xml string.
        /// </summary>
        /// <param name="content">object that shoud be serialized.</param>
        public string Serialize<T>(T content)
        {
            try
            {
                var serializer = new XmlSerializer(content.GetType());

                using var stringWriter = new StringWriter();

                using XmlWriter writer = XmlWriter.Create(stringWriter);

                serializer.Serialize(writer, content);

                return stringWriter.ToString();
            }
            catch (Exception ex)
            {
                throw new SerializerFailException(nameof(content), ex);
            }

        }
    }
}
