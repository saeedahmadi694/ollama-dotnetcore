using Microsoft.AspNetCore.Http;

namespace RAG.AI.Application.Queries.DocumentStatus;
public record DocumentStatusQuery(Guid UniqueId) : IQuery<string>
{
}
//public class CreateTicketCommandValidator : AbstractValidator<UploadDocumentCommand>
//{
//    private readonly List<string> AcceptableExtensions = new()
//    {
//        ".jpg",
//        ".png"
//    };
//    public CreateTicketCommandValidator()
//    {

//        RuleFor(e => e.File)
//            .Must((logo) =>
//            {
//                return AcceptableExtensions.Any(ext => logo.FileName.EndsWith(ext));
//            })
//            .WithMessage("This file format is not supported");
//    }
//}


