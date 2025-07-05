using MediaFormats.RIFF;
using System;
using System.IO;

namespace Test;

internal class Program
{
    static void Main()
    {
        var data = File.ReadAllBytes("DDOS.wav");
        Console.WriteLine(RIFFCoder.IsFormat(data));
        Console.WriteLine(RIFFCoder.Decode(data));
    }
}
