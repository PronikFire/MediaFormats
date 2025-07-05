using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaFormats.RIFF;

public class RIFFPayload(byte[] data, string id) : RIFFChunk(id, (uint)data.Length)
{
    public readonly byte[] data = data;
}
