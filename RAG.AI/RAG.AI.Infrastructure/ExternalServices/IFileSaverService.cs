using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace RAG.AI.Infrastructure.ExternalServices;

public interface IFileSaverService
{
    Task<string> SaveImageToServer(IFormFile file, string folder = "");
    Task<string> GetImageUrl(string file, string folder = "");
    Task<Stream> GetImageStream(string file, string folder = "");
    Task DeleteImageFromServer(string file, string folder = "");
}

