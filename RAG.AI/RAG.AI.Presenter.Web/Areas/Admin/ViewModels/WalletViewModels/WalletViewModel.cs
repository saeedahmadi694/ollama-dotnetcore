using RAG.AI.Application.Queries.Wallets.GetAllWalletsForAdmin;
using RAG.AI.Infrastructure.Dtos.Sliders;
using RAG.AI.Infrastructure.Dtos.Wallets;

namespace RAG.AI.Presenter.Web.Areas.Admin.ViewModels.WalletViewModels
{
    public record WalletViewModel(GetAllWalletsForAdminQuery Query, PagedDto<WalletsForAdminDto> PagingHandler);
}

