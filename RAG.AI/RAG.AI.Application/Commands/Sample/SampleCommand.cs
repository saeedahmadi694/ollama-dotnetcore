namespace RAG.AI.Application.Commands.Sample
{
    public class SampleCommand : IRequest<Unit>
    {
    }

    public class SampleCommandValidator : AbstractValidator<SampleCommand>
    {
        public SampleCommandValidator()
        {
            
        }
    }
}

