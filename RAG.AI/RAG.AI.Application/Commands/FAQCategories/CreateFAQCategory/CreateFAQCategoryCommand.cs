namespace RAG.AI.Application.Commands.FAQCategories.CreateFAQCategory;
public record CreateFAQCategoryCommand(
        string Title,
        bool IsActive
    ) : ICommand<Unit>
{
}
public class CreateFAQCategoryCommandValidator : AbstractValidator<CreateFAQCategoryCommand>
{
    public CreateFAQCategoryCommandValidator()
    {
        //RuleFor(e => e.IsActive)
        //   .NotEmpty()
        //   .NotNull()
        //   .WithMessage("IsActive must have value");
        RuleFor(e => e.Title)
            .NotEmpty()
            .NotNull()
            .WithMessage("Title must have value");
    }
}



