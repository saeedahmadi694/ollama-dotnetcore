using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAG.AI.Infrastructure.Extentions;
public static class StreamExtensions
{
    public static byte[] GetAllBytes(this Stream stream)
    {
        using MemoryStream memoryStream = new MemoryStream();
        stream.CopyTo(memoryStream);
        return memoryStream.ToArray();
    }
}
