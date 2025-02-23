using RAG.AI.Application.Queries.Sliders.GetAllSlidersForAdmin;
using RAG.AI.Infrastructure.Dtos.Sliders;

namespace RAG.AI.Presenter.Web.Areas.Admin.ViewModels.SliderViewModels;

public record SliderViewModel(GetAllSlidersForAdminQuery Query, PagedDto<SlidersForAdminDto> PagingHandler);

