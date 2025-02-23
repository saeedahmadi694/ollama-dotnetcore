using RAG.AI.Domain.Aggregates.FAQCategoryAggregate;
using RAG.AI.Domain.SeedWork;

namespace RAG.AI.Application.Commands.FAQCategories.CreateFAQCategory;
public class CreateFAQCategoryCommandHandler : IRequestHandler<CreateFAQCategoryCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateFAQCategoryCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<Unit> Handle(CreateFAQCategoryCommand request, CancellationToken cancellationToken)
    {

        var item = new FAQCategory(request.Title, request.IsActive);
        await _unitOfWork.FAQCategoryRepository.InsertAsync(item);

        return Unit.Value;
    }
}


