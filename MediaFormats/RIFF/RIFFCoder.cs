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

    /// <summary>
    /// Reads chunks of the first layer.
    /// </summary>
    /// <param name="source">Data as an array of bytes.</param>
    /// <returns>First layer chunks.</returns>
    /// <exception cref="Exception"></exception>
    public static RIFFList Decode(byte[] data)
    {
        if (GetFirstChunk(data) is not RIFFList list || list.id != "RIFF")
            throw new Exception("The data is not RIFF format.");

        if (list.size > data.Length - 8)
            throw new Exception("The data is corrupted.");

        return list;
    }

    internal static RIFFChunk GetFirstChunk(byte[] data)
    {
        string chID = Encoding.ASCII.GetString(data[..4]);
        uint chSize = BitConverter.ToUInt32(data.AsSpan()[4..8]);

        if (chSize > data.Length - 8)
            throw new Exception("The chunk is corrupted.");

        if (chID != "LIST" && chID != "RIFF")
            return new RIFFPayload(data[8..(8 + (int)chSize)], chID);

        var name = Encoding.ASCII.GetString(data[8..12]);

        var subChunks = GetAllChunks(data[12..(8 + (int)chSize)]);

        return new RIFFList(name, subChunks, chID);
    }

    internal static RIFFChunk[] GetAllChunks(byte[] data)
    {
        List<RIFFChunk> subChunks = [];
        for (uint i = 0; i < data.Length;)
        {
            var chunk = GetFirstChunk(data[(int)i..]);
            i += 8 + chunk.size;
            if ((chunk.size % 2) != 0 && i < data.Length)
                i++;
            subChunks.Add(chunk);
        }
        return [.. subChunks];
    }

    /* Maybe later
    public static byte[] Encode(RIFFData data)
    {
        var chunks = new Dictionary<string, byte[]>();

        for (int i = 0; i < source.Length;)
        {
            var chID = BitConverter.ToString(source[i..4]);
            i += 4;
            var chSize = BitConverter.ToInt32(source.AsSpan()[i..4]);
            i += 4;
            var chData = source[i..chSize];
            i += chSize;
            chunks.Add(chID, chData);
        }
    }*/
}