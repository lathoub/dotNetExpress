﻿namespace dotNetExpress.examples;

internal partial class Examples
{
    internal static void BodyParser()
    {
        var app = new Express();
        const int port = 8080;

        //    app.use(Express.json());

        app.Get("/", Express.Json());

        app.Listen(port, () =>
        {
            Console.WriteLine($"Example app listening on port {port}");
        });
    }
}
