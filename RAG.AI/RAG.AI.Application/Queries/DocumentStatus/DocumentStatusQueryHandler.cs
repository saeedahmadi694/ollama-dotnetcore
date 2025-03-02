using RAG.AI.Application.Queries.DocumentStatus;
using RAG.AI.Infrastructure.Persistent.QueryServices.QueryServiceInterfaces;
using System.Threading.Tasks;

public class DocumentStatusQueryHandler : IRequestHandler<DocumentStatusQuery, string>
{
    private readonly IImportJobQueryService _importJobQueryService;

    public DocumentStatusQueryHandler(IImportJobQueryService importJobQueryService)
    {
        _importJobQueryService = importJobQueryService;
    }

    public async Task<string> Handle(DocumentStatusQuery request, CancellationToken cancellationToken)
    {
        var import = await _importJobQueryService.GetAsync(r => r.UniqueId == request.UniqueId);
        if (import is null)
            throw new Exception("inport job not found");



        return import.Status.Name;
    }
}
