using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;

namespace MediaFormats.RIFF.WAV;

public static class WAVCoder
{
    public static WAVClip Decode(RIFFChain riffChain)
    {
        if (riffChain.Format != "WAVE")
            throw new ArgumentException("The data is not WAV format.");

        var fmtChunk = riffChain.subChunks.First(sC => sC.Id == "fmt ") as RIFFPayload
            ?? throw new NullReferenceException("The fmt chunk is missing.");

        var clip = new WAVClip()
        {
            format = BitConverter.ToUInt16(fmtChunk.Data.AsSpan()[..2]),
            numChannels = BitConverter.ToUInt16(fmtChunk.Data.AsSpan()[2..4]),
            sampleRate = BitConverter.ToUInt32(fmtChunk.Data.AsSpan()[4..8]),
            byteRate = BitConverter.ToUInt32(fmtChunk.Data.AsSpan()[8..12]),
            blockAlign = BitConverter.ToUInt16(fmtChunk.Data.AsSpan()[12..14]),
            bitsPerSample = BitConverter.ToUInt16(fmtChunk.Data.AsSpan()[14..16])
        };

        //😭
        if (clip.format != 1)
            throw new Exception("Only PCM is supported.");

        var dataChunk = riffChain.subChunks.First(sC => sC.Id == "data") as RIFFPayload
            ?? throw new NullReferenceException("The data chunk is missing."); ;

        ushort bytesPerSample = (ushort)(clip.bitsPerSample / 8);
        float[] clipData = new float[dataChunk.Data.Length / bytesPerSample];
        for (int i = 0; i < dataChunk.Data.Length; i += bytesPerSample)
        {
            byte[] sampleBytes = dataChunk.Data[i..(i + bytesPerSample)];
            float sample = 0;
            switch (sampleBytes.Length)
            {
                case 1:
                    sample = (sampleBytes[0] - 128) / 128f;
                    break;
                case 2:
                    sample = BitConverter.ToInt16(sampleBytes) / 32768f;
                    break;
                case 3:
                    int intValue = (sampleBytes[2] << 16) | (sampleBytes[1] << 8) | sampleBytes[0];
                    if ((sampleBytes[2] & 0x80) != 0)
                        intValue |= unchecked((int)0xFF000000);
                    sample = intValue / 8388608f;
                    break;
                case 4:
                    if (clip.format == 3)
                        sample = BitConverter.ToSingle(sampleBytes);
                    else
                        sample = BitConverter.ToInt32(sampleBytes) / 2147483648f;
                    break;
                default:
                    throw new Exception("Unsupported audio bit depth.");
            }
            clipData[i / bytesPerSample] = sample;
        }
        clip.data = clipData;

        return clip;
    }
}
