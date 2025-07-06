using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaFormats.RIFF;

public class RIFFPayload(string id, byte[] data) : RIFFChunk(id, (uint)data.Length)
{
    public byte[] Data
    {
        get => data;
        set
        {
            data = value;
            size = (uint)value.Length;
        }
    }

    private byte[] data = data;
}
