using RAG.AI.Infrastructure.Dtos.FAQCategories;

namespace RAG.AI.Application.Queries.FAQCategories.GetFAQCategoryDetail;

public record GetFAQCategoryDetailQuery(int Id) : IQuery<FAQCategoryDto>
{
}

public class GetFAQCategoryDetailQueryValidator : AbstractValidator<GetFAQCategoryDetailQuery>
{
    public GetFAQCategoryDetailQueryValidator()
    {
        RuleFor(e => e.Id)
            .NotEmpty()
            .NotNull()
            .WithMessage("Id must have value");
    }
}



