using RAG.AI.Application.Queries.UserBanks.GetAllUserBanksForAdmin;
using RAG.AI.Infrastructure.Dtos.UserBank;

namespace RAG.AI.Presenter.Web.Areas.Admin.ViewModels.UserBankViewModels;

public record UserBankViewModel(GetAllUserBanksForAdminQuery Query, PagedDto<GetUserBanksForAdminDto> PagingHandler);

