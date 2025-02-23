using RAG.AI.Application.Queries.Users.GetAllUsersForAdmin;
using RAG.AI.Infrastructure.Dtos.Users;

namespace RAG.AI.Presenter.Web.Areas.Admin.ViewModels.UserViewModels;

public record UserViewModel(GetAllUsersForAdminQuery Query, PagedDto<UsersForAdminDto> PagingHandler);

