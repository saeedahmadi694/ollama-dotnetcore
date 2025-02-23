using RAG.AI.Infrastructure.Dtos.FAQCategories;
using RAG.AI.Infrastructure.Persistent.QueryServices.QueryServiceInterfaces;

namespace RAG.AI.Application.Queries.FAQCategories.GetFAQCategoryDetail;

public class GetFAQCategoryDetailQueryHandler : IRequestHandler<GetFAQCategoryDetailQuery, FAQCategoryDto>
{
    private readonly IFAQCategoryQueryService _ticketQueryService;

    public GetFAQCategoryDetailQueryHandler(IFAQCategoryQueryService ticketQueryService)
    {
        _ticketQueryService = ticketQueryService;
    }

    public async Task<FAQCategoryDto> Handle(GetFAQCategoryDetailQuery request, CancellationToken cancellationToken)
    {
        return await _ticketQueryService.GetFAQCategoryDetail(request.Id);
    }
}


