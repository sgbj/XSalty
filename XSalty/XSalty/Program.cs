namespace XSalty
{
    using System;
    using System.IO;
    using System.Xml.Linq;

    class Program
    {
        static void Main(string[] args)
        {
            var xml = File.ReadAllText("message.xml");
            var template = File.ReadAllText("warrant-collection-template.csxml");
            var doc = XDocument.Parse(xml);
            var text = doc.XSalty(template);
            File.WriteAllText("warrant-collection.xml", text);
        }
    }
}
