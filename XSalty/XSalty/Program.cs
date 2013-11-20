namespace XSalty
{
    using System;
    using System.Xml.Linq;
    using System.Xml.XPath;

    class Program
    {
        static void Main(string[] args)
        {
            var xml =
@"<?xml version=""1.0""?>
<box>
    <book>
        <title>Harry Potter and the Philosopher's Stone</title>
        <author>J.K. Rowling</author>
    </book>
    <movie>
        <name>E.T. the Extra-Terrestrial</name>
        <director>Steven Spielberg</director>
    </movie>
    <book>
        <title>Twilight</title>
        <author>Stephenie Meyer</author>
    </book>
</box>";

            var doc = XDocument.Parse(xml);

            var builder = doc.XSalty();

            builder
                .Map("''", "school")
                    .Map("'John Doe High'", "name").Parent()
                    .Map("''", "library")
                        .Map("'Jane Doe Public Library'", "name").Parent()
                        .Map("/box", "bookshelf")
                            .Map("./book", "book")
                                .Map("./title", "@title", includeEverything: true)
                                .Map("./author", "@author", includeEverything: true)
            ;

            Console.WriteLine(builder.Transform(xml));


            //Console.WriteLine(new XDocument().XPathEvaluate("(3+2)*4"));
        }
    }
}

/*
-> school
    'John Doe High' -> name
    -> library
        'Jane Doe Public Library' -> name
        /box -> bookshelf
            ./title -> @title !
            ./author -> @author !
*/