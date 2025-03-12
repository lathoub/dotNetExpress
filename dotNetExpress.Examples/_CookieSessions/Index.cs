﻿using Pynch.dotNetExpress;

namespace dotNetExpress.examples;

internal partial class Examples
{

    internal static async Task CookieSession()
    {
        var app = new Express();
        const int port = 8080;

        // add req.session cookie support
        //app.Use(cookieSession({ "secret:" "manny is cool" }));

        //app.Get("/", async Task (req, res, next) =>
        //{
        //    req.Session.count = (req.Session.count || 0) + 1
        //    res.Send('viewed ' + req.Session.count + ' times\n')
        //});

        await app.Listen(port, () =>
        {
            Console.WriteLine($"Example app listening on port {port}");
        });
    }
}
