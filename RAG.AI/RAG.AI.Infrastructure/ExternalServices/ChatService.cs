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
            شما فقط می‌توانید بر اساس اطلاعات ارائه‌شده در این گفتگو پاسخ دهید. اگر پاسخ را نمی‌دانید، بگویید که اطلاعات کافی در متن داده‌شده وجود ندارد.
            اگر اطلاعات مرتبطی در متن نیست، پیشنهاد دهید که سؤال دقیق‌تر یا اصلاح‌شده‌ای مطرح شود.
            پاسخ‌ها باید به همان زبانی که سؤال مطرح شده داده شوند.
            پاسخ را حداکثر در **۲۰۰ کلمه** ارائه دهید.
            پاسخ‌ها باید در قالب **Markdown** فرمت‌بندی شوند.
        """
        );

        var prompt = new StringBuilder(
            """
        از اطلاعات زیر استفاده کنید:
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
        به پرسش زیر پاسخ دهید:
        ---
        {question}

        **توجه:** پاسخ باید کوتاه و مفید باشد (حداکثر ۲۰۰ کلمه).
        """
        );

        chat.AddUserMessage(prompt.ToString());
        var answer = await chatCompletionService.GetChatMessageContentAsync(chat)!;

        return answer.Content!;
    }
}
