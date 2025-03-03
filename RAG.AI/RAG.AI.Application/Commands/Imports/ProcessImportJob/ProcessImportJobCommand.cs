namespace RAG.AI.Application.Commands.Imports.ProcessExcelImportJob;
public record ProcessImportJobCommand(int JobId) : ICommand<Unit>
{
}



public class ProcessExcelImportJobCommandValidator : AbstractValidator<ProcessImportJobCommand>
{
    public ProcessExcelImportJobCommandValidator()
    {
        RuleFor(e => e.JobId)
            .GreaterThan(0)
            .WithMessage("Job id must be greater than 0");
    }
}