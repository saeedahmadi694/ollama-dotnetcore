using RAG.AI.Infrastructure.Dtos.Common;
using RAG.AI.Infrastructure.Dtos.FAQCategories;
using RAG.AI.Infrastructure.Persistent.QueryServices.QueryServiceInterfaces;

namespace RAG.AI.Application.Queries.FAQCategories.GetAllFAQCategory;

public class GetAllFAQCategoryiesQueryHandler : IRequestHandler<GetAllFAQCategoriesQuery, PagedDto<FAQCategoryDto>>
{
    private readonly IFAQCategoryQueryService _ticketQueryService;

    public GetAllFAQCategoryiesQueryHandler(IFAQCategoryQueryService ticketQueryService)
    {
        _ticketQueryService = ticketQueryService;
    }

    public async Task<PagedDto<FAQCategoryDto>> Handle(GetAllFAQCategoriesQuery request, CancellationToken cancellationToken)
    {
        var filter = new GetAllFAQCategoriesFilter(request.PageNumber, request.PageSize);
        return await _ticketQueryService.GetAllFAQCategorys(filter);
    }
}


