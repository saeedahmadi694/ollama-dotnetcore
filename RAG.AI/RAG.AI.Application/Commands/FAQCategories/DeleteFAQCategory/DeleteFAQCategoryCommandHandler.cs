using RAG.AI.Domain.SeedWork;
using RAG.AI.Infrastructure.Exceptions.FAQCategories;

namespace RAG.AI.Application.Commands.FAQCategories.DeleteFAQCategory;

public class DeleteFAQCategoryCommandHandler : IRequestHandler<DeleteFAQCategoryCommand, Unit>
{
    private readonly IUnitOfWork _uow;

    public DeleteFAQCategoryCommandHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Unit> Handle(DeleteFAQCategoryCommand request, CancellationToken cancellationToken)
    {
        var item = await _uow.FAQCategoryRepository.GetAsync(request.FAQCategoryId);
        if (item is null)
        {
            throw new FAQCategoryNotFoundException(request.FAQCategoryId);
        }
        await _uow.FAQCategoryRepository.DeleteAsync(item.Id);
        return Unit.Value;
    }
}


