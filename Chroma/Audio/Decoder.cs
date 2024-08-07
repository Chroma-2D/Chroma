﻿namespace Chroma.Audio;

using System.Collections.Generic;

public sealed class Decoder
{
    public string Description { get; }
    public string Author { get; }
    public string Url { get; }
        
    public IReadOnlyList<string> SupportedFormats { get; }

    internal Decoder(string description, string author, string url, IReadOnlyList<string> supportedFormats)
    {
        Description = description;
        Author = author;
        Url = url;
        SupportedFormats = supportedFormats;
    }
}