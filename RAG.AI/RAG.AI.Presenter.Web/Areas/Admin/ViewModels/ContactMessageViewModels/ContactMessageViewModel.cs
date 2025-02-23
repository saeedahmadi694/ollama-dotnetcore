using RAG.AI.Application.Queries.ContactMessages.GetAllContactMessages;
using RAG.AI.Infrastructure.Dtos.ContactMessages;

namespace RAG.AI.Presenter.Web.Areas.Admin.ViewModels.ContactMessageViewModels;

public record ContactMessageViewModel(GetAllContactMessagesQuery Query, PagedDto<ContactMessageDto> PagingHandler);

