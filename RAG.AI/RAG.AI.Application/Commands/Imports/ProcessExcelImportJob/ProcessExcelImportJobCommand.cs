namespace RAG.AI.Application.Commands.Imports.ProcessExcelImportJob;
public record ProcessExcelImportJobCommand(int JobId) : ICommand<Unit>
{
}



public class ProcessExcelImportJobCommandValidator : AbstractValidator<ProcessExcelImportJobCommand>
{
    public ProcessExcelImportJobCommandValidator()
    {
        RuleFor(e => e.JobId)
            .GreaterThan(0)
            .WithMessage("Job id must be greater than 0");
    }
}