namespace XSalty
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.XPath;

    public class XSBuilder
    {
        private XSTransform Root { get; set; }

        public XSTransform Map(string source, string destination, bool includeEverything = false)
        {
            Root = new XSTransform(null, source, destination, includeEverything);
            return Root;
        }

        public string Transform(string xml)
        {
            var original = XDocument.Parse(xml);
            var transformed = new XDocument(Root.Transform(original));
            return transformed.ToString();
        }

        public static XSBuilder FromStylesheet(string stylesheet)
        {
            var builder = new XSBuilder();
            return builder;
        }
    }

    public class XSTransform
    {
        public XSTransform Root;
        private List<XSTransform> Children = new List<XSTransform>();
        private string Source;
        private string Destination;
        private bool IncludeEverything;

        public XSTransform(XSTransform root, string source, string destination, bool includeEverything = false)
        {
            Root = root;
            Source = source;
            Destination = destination;
            IncludeEverything = includeEverything;
        }

        public XSTransform Map(string source, string destination, bool includeEverything = false)
        {
            var transform = new XSTransform(this, source, destination, includeEverything);
            Children.Add(transform);
            return destination.StartsWith("@") ? this : transform;
        }

        public XSTransform Parent()
        {
            return Root;
        }

        public IEnumerable<XObject> Transform(XObject o)
        {
            var matches = ((XNode)o).XPathEvaluate(Source);
            if (matches is IEnumerable && !(matches is string))
            {
                foreach (XObject match in (IEnumerable)matches)
                {
                    if (Source.XPathParts().Last().StartsWith("@"))
                    {

                        yield return Destination.StartsWith("@") ? (XObject)
                            new XAttribute(Destination.TrimStart('@'), ((XAttribute)match).Value) :
                            new XElement(Destination, ((XAttribute)match).Value);
                    }

                    else
                    {

                        yield return Destination.StartsWith("@") ? (XObject)
                            new XAttribute(Destination.TrimStart('@'), ((XElement)match).Value) :
                            IncludeEverything ?
                                new XElement((XElement)match) :
                                new XElement(Destination, Children.SelectMany(c => c.Transform(match)));
                    }
                }
            }
            else
            {
                yield return Destination.StartsWith("@") ? (XObject)
                    new XAttribute(Destination.TrimStart('@'), matches) :
                    new XElement(Destination, Children.SelectMany(c => c.Transform(o)).Concat(new[] { new XText(matches.ToString()) }));
            }
        }
    }

    public static class XSaltyUtility
    {
        public static string[] XPathParts(this string xpath)
        {
            return xpath.Trim('/').Split('/');
        }

        public static XSBuilder XSalty(this XDocument document)
        {
            return new XSBuilder();
        }
    }
}