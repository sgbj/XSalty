XSalty
======
XSalty is a lightweight API for performing XSLT transformations with Razor syntax.

Example
-------
Using XSalty is easy:

```csharp
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
```
            
Output:

```html
<!doctype html>
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
            <tr>
                <td>John Doe</td>
                <td>37</td>
            </tr>
            <tr>
                <td>Jane Doe</td>
                <td>35</td>
            </tr>
        </table>
    </body>
</html>
```
