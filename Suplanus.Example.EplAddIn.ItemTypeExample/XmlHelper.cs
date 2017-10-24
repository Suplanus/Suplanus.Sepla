using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Suplanus.Example.EplAddIn.ItemTypeExample
{
    public static class XmlHelper
    {
        public static T Read<T>(string filename)
        {
            var serializer = new XmlSerializer(typeof(T));
            var reader = new StreamReader(filename);

            var returnValue = (T) serializer.Deserialize(reader);
            reader.Close();
            reader.Dispose();
            return returnValue;
        }

        public static void Write<T>(T obj, string filename)
        {
            var serializer = new XmlSerializer(typeof(T));
            var namespaces = new XmlSerializerNamespaces();

            var settings = new XmlWriterSettings {Indent = true, Encoding = Encoding.UTF8};
            var writer = XmlWriter.Create(filename, settings);

            serializer.Serialize(writer, obj, namespaces);
            writer.Close();
        }
    }
}