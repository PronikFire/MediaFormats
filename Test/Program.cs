using MediaFormats.RIFF;
using MediaFormats.RIFF.WAV;
using System;
using System.IO;

namespace Test;

internal class Program
{
    static void Main()
    {
        var data = File.ReadAllBytes("DDOS.wav");
        var riffChain = RIFFCoder.Decode(data);
        Console.WriteLine(string.Join('\n', WAVCoder.Decode(riffChain).info));
    }
}
