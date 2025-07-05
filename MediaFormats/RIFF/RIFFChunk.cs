using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaFormats.RIFF;

public class RIFFChunk
{
    public string id;
    public readonly uint size;
    public RIFFChunk(string id, uint size)
    {
        if (id.Length > 4)
            throw new Exception("The ID cannot be longer than 4.");
        this.id = id;
        this.size = size;
    }
}
