using System.IO;
using System.Reflection;
using System.Xml.Xsl;

namespace System.Xml
{
   //fix self closing tag on xml, required for serialization
   public class SelfClosingTagsFixer
    {
        private static readonly XslCompiledTransform transform ;

        static SelfClosingTagsFixer()
        {
            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("BaseLib.Core.Resources.fix-self-closing-tags.xslt") 
                ?? throw new NullReferenceException("Couldn't load transformation from assembly resources");
            using var reader = XmlReader.Create(stream);
            transform = new XslCompiledTransform();
            transform.Load(reader, new XsltSettings(true, true), new XmlUrlResolver());

        }

        private static void Fix(Stream source, Stream target)
        {
            var writer = XmlWriter.Create(target, new XmlWriterSettings());

            using (var reader = XmlReader.Create(source))
            {
                var arguments = new XsltArgumentList();
                transform.Transform(reader, arguments, writer);
                writer.Flush();
            }
        }

        public static Stream Fix(Stream source)
        {
            var target = new MemoryStream();
            Fix(source, target);
            target.Position = 0;
            return target;
        }
    }
}
