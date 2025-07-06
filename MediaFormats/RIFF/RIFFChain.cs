using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaFormats.RIFF;

public class RIFFChain(string format, RIFFChunk[] subChunks)
{
    public string Format 
    {
        get => format;
        set
        {
            if (value.Length > 4)
                throw new ArgumentException($"Format cannot be longer than 4 characters.", nameof(value));
            format = value;
        }
    }

    public RIFFChunk[] subChunks = subChunks;
    private string format = format;
}
