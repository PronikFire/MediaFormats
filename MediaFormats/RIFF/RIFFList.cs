using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaFormats.RIFF;

public class RIFFList(string name, RIFFChunk[] subChunks, string id) : RIFFChunk(id, (uint)subChunks.Sum(sC => sC.size + 8) + 4)
{
    public readonly string name = name;
    public readonly RIFFChunk[] subChunks = subChunks;
}
