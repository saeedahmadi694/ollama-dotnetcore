using RAG.AI.Application.Queries.Operations.GetAllOperationsForAdmin;
using RAG.AI.Infrastructure.Dtos.Operations;

namespace RAG.AI.Presenter.Web.Areas.Admin.ViewModels.OperationViewModels;

public record OperationViewModel(GetAllOperationsForAdminQuery Query, PagedDto<OperationsForAdminDto> PagingHandler);

