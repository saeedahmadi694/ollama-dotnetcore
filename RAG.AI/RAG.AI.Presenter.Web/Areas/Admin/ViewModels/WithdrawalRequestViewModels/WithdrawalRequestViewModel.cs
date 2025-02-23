using RAG.AI.Application.Queries.WithdrawalRequests.GetAllWithdrawalRequestsForAdmin;
using RAG.AI.Infrastructure.Dtos.WithdrawalRequests;

namespace RAG.AI.Presenter.Web.Areas.Admin.ViewModels.WithdrawalRequestViewModels;

public record WithdrawalRequestViewModel(GetAllWithdrawalRequestsForAdminQuery Query, PagedDto<WithdrawalRequestDto> PagingHandler);

