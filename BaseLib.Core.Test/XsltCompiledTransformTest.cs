using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Xsl;
using BaseLib.Core.Extensions;

namespace BaseLib.Core.Test
{
    [TestClass]
    public class XsltCompiledTransformTest
    {
        private const string resourcePath = "BaseLib.Core.Test.Resources";

        [TestMethod]
        public void Transform_Default()
        {
            //cargue books
            var xslt = GetXsltFromResource("default.xslt");
            using (var source = GetStreamFromResource("books.xml"))
            using( var target = xslt.Transform(source))
            using (var reader = new StreamReader(target))
            {
                Console.WriteLine(reader.ReadToEnd());
            }
        }

        [TestMethod]
        public void Transform_Multiple_Documents()
        {

            //cargue books
            var xslt = GetXsltFromResource("multiple-sources.xslt");
            using (var source = GetStreamFromResource("books.xml"))
            using (var source2 = GetStreamFromResource("employees.xml"))
            {
                //se agrega los otros documentos en los argumentos del transform
                XsltArgumentList arguments = new XsltArgumentList();
                arguments.AddNavigatorParam("secondarySource", "", source2);

                using (var target = xslt.Transform(source, arguments))
                using (var reader = new StreamReader(target))
                {
                    Console.WriteLine(reader.ReadToEnd());
                }
            }
        }

        public XslCompiledTransform GetXsltFromResource(string fileName)
        {
            using (var reader = GetXmlReaderFromResource(fileName))
            {
                XslCompiledTransform xslt = new XslCompiledTransform();
                xslt.Load(reader);
                return xslt;
            }
        }

        public XmlReader GetXmlReaderFromResource(string fileName)
        {
            var stream = GetStreamFromResource(fileName);
            return XmlReader.Create(stream);
        }

        public Stream GetStreamFromResource(string fileName)
        {
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"{resourcePath}.{fileName}");
            return stream;
        }

    }
}
