using clsSocialServicesDataAccess.Posts;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace clsSocialServicesBussiness
{
    public class clsAiRecommendationService
    {
        private readonly string _apiKey;
        private readonly IPostRepository _postRepo;

        public clsAiRecommendationService(IPostRepository postRepo, IConfiguration configuration)
        {
            _postRepo = postRepo;
            _apiKey = configuration["Gemini:ApiKey"]!;
        }

        public async Task<string> GetRecommendations(string userMessage, int userId)
        {
            var posts = _postRepo.GetAllPosts(userId);
            string postsJson = JsonSerializer.Serialize(posts);

            var client = new HttpClient();

            string prompt = $@"
أنت نظام توصية خدمات. مهمتك اختيار أنسب البوستات لطلب المستخدم.

طلب المستخدم:
{userMessage}

البوستات المتاحة (JSON):
{postsJson}

معايير الاختيار (مرتبة حسب الأولوية):
1. تطابق نوع الخدمة المطلوبة
2. تطابق أو قرب الموقع الجغرافي في حال كان في نفس المدينة
3. التقييم والمصداقية إن وُجدا

تعليمات:
- إذا حدد المستخدم عدداً معيناً، التزم به تماماً
- إذا لم يحدد، رشح 3 بوستات
- إذا لم تجد بوستات مناسبة، أرجع قائمة فارغة
- رتّب النتائج من الأنسب للأقل

أرجع JSON فقط، بدون أي نص قبله أو بعده، بهذا الشكل الحرفي:
{{""postIds"": [36, 28, 23]}}
";

            var body = new
            {
                contents = new[]
                {
            new
            {
                parts = new[]
                {
                    new { text = prompt }
                }
            }
        }
            };

            var response = await client.PostAsJsonAsync(
                $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={_apiKey}",
                body
            );

            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return $"Error: {result}";

            var jsonDoc = JsonDocument.Parse(result);
            string text = jsonDoc.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString()!;

            return text;
        }

    }
}
