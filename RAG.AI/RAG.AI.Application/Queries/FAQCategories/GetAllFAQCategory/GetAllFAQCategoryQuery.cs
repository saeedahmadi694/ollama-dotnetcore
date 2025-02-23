

using RAG.AI.Infrastructure.Dtos.Common;
using RAG.AI.Infrastructure.Dtos.FAQCategories;

namespace RAG.AI.Application.Queries.FAQCategories.GetAllFAQCategory;

public record GetAllFAQCategoriesQuery(int PageNumber = 1, int PageSize = 10) : IQuery<PagedDto<FAQCategoryDto>>
{
}


