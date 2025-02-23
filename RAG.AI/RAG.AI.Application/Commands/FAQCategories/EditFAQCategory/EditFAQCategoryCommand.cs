

namespace RAG.AI.Application.Commands.FAQCategories.EditFAQCategory;

public record EditFAQCategoryCommand(
        int FAQCategoryId,
        string Title, bool IsActive
    ) : ICommand<Unit>
{
}
public class EditFAQCategoryCommandValidator : AbstractValidator<EditFAQCategoryCommand>
{
    public EditFAQCategoryCommandValidator()
    {
        RuleFor(e => e.FAQCategoryId)
         .NotEmpty()
         .NotNull()
         .WithMessage("FirstName must have value");
        RuleFor(e => e.IsActive)
           .NotEmpty()
           .NotNull()
           .WithMessage("IsActive must have value");
        RuleFor(e => e.Title)
            .NotEmpty()
            .NotNull()
            .WithMessage("Title must have value");
    }
}


