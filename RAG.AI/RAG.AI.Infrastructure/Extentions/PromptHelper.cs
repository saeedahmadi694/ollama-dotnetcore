using Microsoft.SemanticKernel.ChatCompletion;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace RAG.AI.Infrastructure.Extentions;

public static class PromptHelper
{

    public static string ToPrompt(this string question, string[] contexts)
    {
        var sb = new StringBuilder();
        sb.Append(
         """
            شما فقط می‌توانید بر اساس اطلاعات ارائه‌شده در این گفتگو پاسخ دهید. اگر پاسخ را نمی‌دانید، بگویید که اطلاعات کافی در متن داده‌شده وجود ندارد.
            اگر اطلاعات مرتبطی در متن نیست، پیشنهاد دهید که سؤال دقیق‌تر یا اصلاح‌شده‌ای مطرح شود.
            پاسخ‌ها باید به همان زبانی که سؤال مطرح شده داده شوند.
            پاسخ را حداکثر در **۲۰۰ کلمه** ارائه دهید.
            پاسخ‌ها باید در قالب **Markdown** فرمت‌بندی شوند.
         """
        );

        sb.AppendLine(
        """
            از اطلاعات زیر استفاده کنید:
            =====
        """
        );

        foreach (var text in contexts)
        {
            sb.AppendLine("---");
            sb.AppendLine(text);
        }

        sb.AppendLine(
            $"""

                =====
                به پرسش زیر پاسخ دهید:
                ---
                {question}

                **توجه:** پاسخ باید کوتاه و مفید باشد (حداکثر ۲۰۰ کلمه).
             """
        );

        return sb.ToString();
    }

}


