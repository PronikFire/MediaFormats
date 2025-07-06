using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaFormats.RIFF.WAV;

public class WAVClip()
{
    public RIFFChunk[] info = [];
    public ushort format;
    public ushort numChannels;
    public uint sampleRate;
    public uint byteRate;
    public ushort blockAlign;
    public ushort bitsPerSample;
    public float[] data = [];
}
