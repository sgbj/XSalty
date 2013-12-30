namespace XSalty
{
    using System;
    using System.Xml.Linq;

    class Program
    {
        static void Main(string[] args)
        {
            var doc =
                new XDocument(
                    new XElement("customers",
                        new XElement("person",
                            new XElement("name", "John Doe"),
                            new XElement("age", 37)),
                        new XElement("person",
                            new XElement("name", "Jane Doe"),
                            new XElement("age", 35))));
            var template =
@"<!doctype html>
<html>
    <head>
        <title>Customers</title>
    </head>
    <body>
        <table>
            <tr>
                <th>Name</th>
                <th>Age</th>
            </tr>
            @foreach (var person in Eval(""customers/person""))
            {
            <tr>
                <td>@Eval(""string(name)"", person)</td>
                <td>@Eval(""number(age)"", person)</td>
            </tr>
            }
        </table>
    </body>
</html>";
            var output = doc.XSalty(template);
            Console.WriteLine(output);
        }
    }
}
