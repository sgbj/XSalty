namespace XSalty
{
    using RazorEngine;
    using RazorEngine.Configuration;
    using RazorEngine.Templating;
    using System.Linq;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.XPath;

    public static class XSaltyUtility
    {
        public static string XSalty(this XDocument doc, string template)
        {
            var config = new TemplateServiceConfiguration { BaseTemplateType = typeof(XSaltyTemplate<>) };
            var service = new TemplateService(config);
            Razor.SetTemplateService(service);
            return Razor.Parse(template, doc);
        }
    }

    public abstract class XSaltyTemplate<T> : TemplateBase<T> where T : XDocument
    {
        public dynamic Eval(string xpath, XNode node = null)
        {
            var namespaceManager = new XmlNamespaceManager(new NameTable());
            foreach (var element in Model.Descendants())
                foreach (var attribute in element.Attributes().Where(a => a.IsNamespaceDeclaration))
                    namespaceManager.AddNamespace(attribute.Name.LocalName, XNamespace.Get(attribute.Value).NamespaceName);
            return (node ?? Model).XPathEvaluate(xpath, namespaceManager);
        }
    }
}
