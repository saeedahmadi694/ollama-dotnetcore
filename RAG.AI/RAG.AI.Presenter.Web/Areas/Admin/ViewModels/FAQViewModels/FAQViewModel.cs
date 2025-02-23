using RAG.AI.Application.Queries.FAQs.GetAllFAQs;
using RAG.AI.Infrastructure.Dtos.FAQs;

namespace RAG.AI.Presenter.Web.Areas.Admin.ViewModels.FAQViewModels
{
    public record FAQViewModel(GetAllFAQsQuery Query, PagedDto<FAQDto> PagingHandler);
}

