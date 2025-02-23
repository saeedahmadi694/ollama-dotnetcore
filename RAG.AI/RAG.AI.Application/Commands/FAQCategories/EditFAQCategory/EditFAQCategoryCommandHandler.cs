using RAG.AI.Domain.SeedWork;
using RAG.AI.Infrastructure.Exceptions.FAQCategories;

namespace RAG.AI.Application.Commands.FAQCategories.EditFAQCategory;

public class EditFAQCategoryCommandHandler : IRequestHandler<EditFAQCategoryCommand, Unit>
{
    private readonly IUnitOfWork _uow;

    public EditFAQCategoryCommandHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Unit> Handle(EditFAQCategoryCommand request, CancellationToken cancellationToken)
    {
        var item = await _uow.FAQCategoryRepository.GetAsync(request.FAQCategoryId);
        if (item is null)
        {
            throw new FAQCategoryNotFoundException(request.FAQCategoryId);
        }
        item.Update(request.Title, request.IsActive);
        _uow.FAQCategoryRepository.Update(item);
        return Unit.Value;
    }
}


