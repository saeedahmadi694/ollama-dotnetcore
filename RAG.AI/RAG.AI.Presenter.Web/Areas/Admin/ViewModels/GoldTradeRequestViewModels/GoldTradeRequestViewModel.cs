using RAG.AI.Application.Queries.GoldTradeRequests.GetAllGoldTradeRequestsForAdmin;
using RAG.AI.Infrastructure.Dtos.GoldTradeRequests;

namespace RAG.AI.Presenter.Web.Areas.Admin.ViewModels.GoldTradeRequestViewModels;

public record GoldTradeRequestViewModel(GetAllGoldTradeRequestsForAdminQuery Query, PagedDto<GoldTradeRequestDto> PagingHandler);

