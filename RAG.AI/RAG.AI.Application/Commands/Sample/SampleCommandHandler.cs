namespace RAG.AI.Application.Commands.Sample
{
    public class SampleCommandHandler : IRequestHandler<SampleCommand, Unit>
    {
        public async Task<Unit> Handle(SampleCommand request, CancellationToken cancellationToken)
        {
            await Task.Delay(500);
            return Unit.Value;
        }
    }
}

