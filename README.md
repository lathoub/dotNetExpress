![example workflow](https://github.com/pynch-tv/dotNetExpress/actions/workflows/dotnet.yml/badge.svg)
# .net Express

## Getting started

### Hello world example

```cs
static void HelloWorld()
{
    var app = new Express();
    const int port = 8080;

    app.Get("/", (req, res, next) =>
    {
        res.Send("Hello World");
    });

    app.Listen(port, () =>
    {
        Console.WriteLine($"Example app listening on port {port}");
    });
}
```
More examples in the [Examples](https://github.com/pynch-tv/dotNetExpress/tree/main/examples) folder

## Optional Dependencies

For Multipart POST commands

- Multipart Parser from [Http-Multipart-Data-Parser](https://github.com/Http-Multipart-Data-Parser/Http-Multipart-Data-Parser)
- Microsoft.IO.RecyclableMemoryStream
