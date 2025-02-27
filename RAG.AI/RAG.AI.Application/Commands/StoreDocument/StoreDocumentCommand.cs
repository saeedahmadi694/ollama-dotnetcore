using Microsoft.AspNetCore.Http;
using RAG.AI.Infrastructure.Dtos.Common;

namespace RAG.AI.Application.Commands.StoreDocument;
public record StoreDocumentCommand(
        Document Doc) : ICommand<Unit>
{
}
//public class StoreDocumentCommandValidator : AbstractValidator<StoreDocumentCommand>
//{
//    private readonly List<string> AcceptableExtensions = new()
//    {
//        ".jpg",
//        ".png"
//    };
//    public StoreDocumentCommandValidator()
//    {

//        RuleFor(e => e.File)
//            .Must((logo) =>
//            {
//                return AcceptableExtensions.Any(ext => logo.FileName.EndsWith(ext));
//            })
//            .WithMessage("This file format is not supported");
//    }
//}


