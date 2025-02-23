using RAG.AI.Application.Queries.WalletChargeRequests.GetAllWalletChargeRequestsForAdmin;
using RAG.AI.Infrastructure.Dtos.WalletChargeRequests;

namespace RAG.AI.Presenter.Web.Areas.Admin.ViewModels.WalletChargeRequestViewModels;

public record WalletChargeRequestViewModel(GetAllWalletChargeRequestsForAdminQuery Query, PagedDto<WalletChargeRequestDto> PagingHandler);

