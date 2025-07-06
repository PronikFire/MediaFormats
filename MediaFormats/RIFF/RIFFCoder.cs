using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace MediaFormats.RIFF;

public static class RIFFCoder
{
    public static bool IsFormat(byte[] data) => data.Length >= 4 && Encoding.ASCII.GetString(data[..4]) == "RIFF";

    public static RIFFChain Decode(byte[] data)
    {
        if (Encoding.ASCII.GetString(data[..4]) != "RIFF")
            throw new ArgumentException("The data is not RIFF format.");

        string format = Encoding.ASCII.GetString(data[8..12]);

        var subChunks = GetAllChunks(data[12..]);

        return new RIFFChain(format, subChunks);
    }

    internal static RIFFChunk GetFirstChunk(byte[] data)
    {
        string chID = Encoding.ASCII.GetString(data[..4]);
        uint chSize = BitConverter.ToUInt32(data.AsSpan()[4..8]);

        if (chSize > data.Length - 8)
            throw new Exception("The chunk is corrupted.");

        if (chID != "LIST")
            return new RIFFPayload(chID, data[8..(8 + (int)chSize)]);

        var name = Encoding.ASCII.GetString(data[8..12]);

        var subChunks = GetAllChunks(data[12..(8 + (int)chSize)]);

        return new RIFFList(name, subChunks);
    }

    internal static RIFFChunk[] GetAllChunks(byte[] data)
    {
        List<RIFFChunk> subChunks = [];
        for (uint i = 0; i < data.Length;)
        {
            var chunk = GetFirstChunk(data[(int)i..]);
            i += 8 + chunk.Size;
            if ((chunk.Size % 2) != 0 && i < data.Length)
                i++;
            subChunks.Add(chunk);
        }
        return [.. subChunks];
    }

    /*public static byte[] Encode(RIFFChain chain)
    {
        List<byte> data = [];
        data.AddRange(Encoding.ASCII.GetBytes("RIFF"));
        data.AddRange(BitConverter.GetBytes(4 + (uint)chain.subChunks.Sum(sC => (int)sC.Size + 8)));
        
    }*/
}