using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaFormats.RIFF;

public class RIFFList(string name, RIFFChunk[] subChunks) : RIFFChunk("LIST", (uint)subChunks.Sum(sC => sC.Size + 8) + 4)
{
    public string Name
    {
        get => name;
        set
        {
            if (value.Length > 4)
                throw new ArgumentException($"Name cannot be longer than 4 characters.", nameof(value));
            name = value;
        }
    }

    public RIFFChunk[] SubChunks
    {
        get => subChunks;
        set
        {
            subChunks = value;
            size = (uint)subChunks.Sum(sC => sC.Size + 8) + 4;
        }
    }

    protected string name = name;
    private RIFFChunk[] subChunks = subChunks;
}
