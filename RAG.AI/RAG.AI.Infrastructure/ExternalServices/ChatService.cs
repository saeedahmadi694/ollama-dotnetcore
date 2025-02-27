using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using System.Text;

namespace RAG.AI.Infrastructure.ExternalServices;


public class ChatService(Kernel kernel) : IChatService
{
    public async Task<string> AskRaggedQuestion(string question, string[] contexts)
    {
        var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

        var chat = new ChatHistory(
            """
            شما فقط می‌توانید از اطلاعات ارائه‌شده در این چت برای پاسخ به سؤالات استفاده کنید. اگر پاسخ را نمی‌دانید، پیشنهاد دهید که سؤال اصلاح شود.
            به عنوان مثال، اگر کاربر بپرسد "پایتخت فرانسه چیست؟" و در این چت اطلاعاتی درباره‌ی فرانسه وجود نداشته باشد، باید پاسخی مانند "این اطلاعات در متن داده‌شده موجود نیست" بدهید.
            به شما بخش‌هایی از متن کتاب‌های مختلف داده می‌شود تا به عنوان زمینه استفاده کنید.
            باید به همان زبانی که سؤال پرسیده شده است پاسخ دهید.
            شما پاسخ‌ها را در قالب Markdown ارائه خواهید داد.

            """
        );

        var prompt = new StringBuilder(
            """
            با استفاده از اطلاعات زیر:
            =====

            """
        );
        foreach (var text in contexts)
        {
            prompt.AppendLine("---");
            prompt.AppendLine(text);
        }

        prompt.AppendLine(
            $"""

            =====
            به سوال زیر پاسخ دهید:
            ---
            {question}
            """
        );

        chat.AddUserMessage(prompt.ToString());
        var answer = await chatCompletionService.GetChatMessageContentAsync(chat)!;

        return answer.Content!;
    }
}
