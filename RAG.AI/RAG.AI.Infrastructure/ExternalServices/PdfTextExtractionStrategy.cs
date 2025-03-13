namespace RAG.AI.Infrastructure.ExternalServices;



//public class PdfTextExtractionStrategy : ITextExtractionStrategy
//{
//    public List<Dtos.Common.Page> ExtractText(byte[] fileBytes)
//    {
//        var ocr = new IronTesseract();
//        ocr.Language = OcrLanguage.Persian;

//        using var input = new OcrInput(fileBytes);
//        var result = ocr.Read(input);
//        var pages = new List<Dtos.Common.Page>();

//        var pageIndex = 1;
//        foreach (var page in result.Pages)
//        {
//            pages.Add(new Dtos.Common.Page(page.Text, pageIndex++));
//        }

//        return pages;
//    }
//}
