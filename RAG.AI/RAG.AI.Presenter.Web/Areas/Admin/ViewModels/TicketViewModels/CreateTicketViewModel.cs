using RAG.AI.Application.Commands.Tickets.CreateTicketByAdmin;
using RAG.AI.Infrastructure.Dtos.TicketCategories;
using Microsoft.CodeAnalysis.Elfie.Extensions;

namespace RAG.AI.Presenter.Web.Areas.Admin.ViewModels.TicketViewModels;

public record CreateTicketViewModel(CreateTicketByAdminCommand Command, IReadOnlyList<TicketCategoryDto>? Categories);

