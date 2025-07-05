using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaFormats.RIFF;

public class RIFFData(string format, RIFFChunk[] subChunks)
{
    public string format = format;
    public RIFFChunk[] subChunks = subChunks;
}
