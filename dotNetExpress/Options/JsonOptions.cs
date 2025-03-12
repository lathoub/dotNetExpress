﻿namespace Pynch.dotNetExpress.Options;

public class jsonOptions
{
    /// <summary>
    /// Enables or disables handling deflated (compressed) bodies;
    /// when disabled, deflated bodies are rejected.	
    /// </summary>
    public bool Inflate;

    /// <summary>
    /// Controls the maximum request body size. If this is a number, then
    /// the value specifies the number of bytes; if it is a string, the value
    /// is passed to the bytes library for parsing.	
    /// </summary>
    public int Limit;

    /// <summary>
    /// The reviver option is passed directly to JSON.parse as the second argument.
    /// You can find more information on this argument in the MDN documentation about JSON.parse.	
    /// </summary>
    public int Reviver;

    /// <summary>
    /// Enables or disables only accepting arrays and objects; when disabled
    /// will accept anything JSON.parse accepts.	
    /// </summary>
    public bool Strict;

    /// <summary>
    /// This is used to determine what media type the middleware will parse. This
    /// option can be a string, array of strings, or a function. If not a function,
    /// type option is passed directly to the type-is library and this can be an extension
    /// name (like json), a mime type (like application/json), or a mime type with a
    /// wildcard (like */* or */json). If a function, the type option is called as fn(req)
    /// and the request is parsed if it returns a truthy value.	
    /// </summary>
    public string Type;

    /// <summary>
    /// This option, if supplied, is called as verify(req, res, buf, encoding), where buf is
    /// a Buffer of the raw request body and encoding is the encoding of the request. The
    /// parsing can be aborted by throwing an error.	
    /// </summary>
    public int Verify;

}
