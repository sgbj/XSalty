XSalty
======
XSalty is a lightweight fluent API for performing XSLT transformations.

It comes with its own syntax for creating stylesheets:

```
-> school
    'John Doe High' -> name
    -> library
        'Jane Doe Public Library' -> name
        /box -> bookshelf
            ./title -> @title !
            ./author -> @author !
```

Or if you prefer creating them with code:

```csharp
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
				.Map("/box", "bookshelf")
					.Map("./book", "book")
						.Map("./title", "@title", includeEverything: true)
						.Map("./author", "@author", includeEverything: true)
            ;

            Console.WriteLine(builder.Transform(xml));
```

Output:
```xml
<bookshelf>
  <book title="Harry Potter and the Philosopher's Stone" author="J.K. Rowling" />
  <book title="Twilight" author="Stephenie Meyer" />
</bookshelf>
```