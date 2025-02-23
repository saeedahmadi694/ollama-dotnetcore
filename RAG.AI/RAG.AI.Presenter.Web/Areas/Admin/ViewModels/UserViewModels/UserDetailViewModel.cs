using RAG.AI.Infrastructure.Dtos.UserBank;
using RAG.AI.Infrastructure.Dtos.Users;

namespace RAG.AI.Presenter.Web.Areas.Admin.ViewModels.UserViewModels;

public record UserDetailViewModel(UserDetailDto user, PagedDto<GetUserBanksForAdminDto> PagingHandler);

