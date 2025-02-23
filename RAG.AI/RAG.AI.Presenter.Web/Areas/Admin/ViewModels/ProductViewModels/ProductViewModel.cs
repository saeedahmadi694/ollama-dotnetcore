using RAG.AI.Application.Queries.Products.GetAllProduct;
using RAG.AI.Infrastructure.Dtos.Products;

namespace RAG.AI.Presenter.Web.Areas.Admin.ViewModels.ProductViewModels;

public record ProductViewModel(GetAllProductsForAdminQuery Query, PagedDto<ProductForAdminDto> PagingHandler);

