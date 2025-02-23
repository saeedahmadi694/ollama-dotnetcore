using RAG.AI.Application.Queries.Comments.GetAllComments;
using RAG.AI.Infrastructure.Dtos.Comments;

namespace RAG.AI.Presenter.Web.Areas.Admin.ViewModels.CommentViewModels;

public record CommentViewModel(GetAllCommentsQuery Query, PagedDto<CommentDto> PagingHandler);

