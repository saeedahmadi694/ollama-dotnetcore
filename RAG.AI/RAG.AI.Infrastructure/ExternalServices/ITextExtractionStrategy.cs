
using Microsoft.Extensions.VectorData;
using RAG.AI.Infrastructure.Dtos.Common;

namespace RAG.AI.Infrastructure.ExternalServices;

public interface ITextExtractionStrategy
{
    List<Page> ExtractText(byte[] fileBytes);
}