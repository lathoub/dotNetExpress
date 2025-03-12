﻿using Pynch.dotNetExpress;

namespace dotNetExpress.examples;

internal partial class Examples
{
    internal static async Task BodyParser()
    {
        var app = new Express();
        const int port = 8080;

//        app.Use(Express.Json());

        app.Post("/v1/servers/XT2/subscribe", Express.Json(),  async Task (req, res, next) =>
        {
            await res.Send("Hello World");
        });

        await app.Listen(port, () =>
        {
            Console.WriteLine($"Example app listening on port {port}");
        });
    }
}
