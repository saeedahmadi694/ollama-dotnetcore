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
            پاسخ‌ها باید به همان زبانی که سؤال مطرح شده داده شوند.
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

        """
        );

        chat.AddUserMessage(prompt.ToString());
        var answer = await chatCompletionService.GetChatMessageContentAsync(chat)!;

        return answer.Content!;
    }
}
