using RAG.AI.Application.Queries.Tickets.GetAllTicketsForAdmin;
using RAG.AI.Infrastructure.Dtos.Tickets;

namespace RAG.AI.Presenter.Web.Areas.Admin.ViewModels.TicketViewModels;

public record TicketViewModel(GetAllTicketsForAdminQuery Query, PagedDto<GetTicketsForAdminDto> PagingHandler);

