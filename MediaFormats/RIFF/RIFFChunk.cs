using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaFormats.RIFF;

public class RIFFChunk
{
    public string Id
    {
        get => id;
        set
        {
            if (value.Length > 4)
                throw new ArgumentException($"ID cannot be longer than 4 characters.", nameof(value));
            id = value;
        }
    }
    internal uint Size => size;

    private string id;
    protected uint size;
    
    public RIFFChunk(string id, uint size)
    {
        if (id.Length > 4)
            throw new ArgumentException($"ID cannot be longer than 4 characters.", nameof(id));
        this.id = id;
        this.size = size;
    }
}
