using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace BaseLib.Core.Extensions
{
    public static class XslExtensions
    {
        public static Stream Transform(this XslCompiledTransform xslt, Stream source, XsltArgumentList arguments = null)
        {
            var output = new MemoryStream();
            using (var sourceReader = XmlReader.Create(source))
            using (var outputWriter = XmlWriter.Create(output))
            {
                xslt.Transform(sourceReader, arguments, outputWriter);
                outputWriter.Flush();
                output.Position = 0;
            }
            return output;
        }

        public static void AddNavigatorParam(this XsltArgumentList arguments, string name, string namespaceUri, Stream stream)
        {
            var document = new XPathDocument(stream);
            var navigator = document.CreateNavigator();
            arguments.AddParam(name, namespaceUri, navigator);

        }
    }
}
