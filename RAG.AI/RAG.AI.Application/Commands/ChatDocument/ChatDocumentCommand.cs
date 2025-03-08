using Microsoft.AspNetCore.Http;
using NPOI.HSSF.Record;

namespace RAG.AI.Application.Commands.ChatDocument;
public record ChatDocumentCommand(string Query, List<string> DocumentIds) : ICommand<string>
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


