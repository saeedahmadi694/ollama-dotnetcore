

namespace RAG.AI.Application.Commands.FAQCategories.DeleteFAQCategory;

public record DeleteFAQCategoryCommand(int FAQCategoryId) : ICommand<Unit>
{
}

public class DeleteFAQCategoryCommandValidator : AbstractValidator<DeleteFAQCategoryCommand>
{
    public DeleteFAQCategoryCommandValidator()
    {
        RuleFor(e => e.FAQCategoryId)
            .NotEmpty()
            .NotNull()
            .WithMessage("FAQCategoryId must have value");
    }
}


