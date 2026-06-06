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
أنت مساعد ذكي متخصص في توصية الخدمات. مهمتك تحليل طلب المستخدم وترشيح أفضل 3 بوستات من القائمة المتاحة.

طلب المستخدم: {userMessage}

البوستات المتاحة:
{postsJson}

قواعد الترشيح:
- ركز على تطابق نوع الخدمة مع طلب المستخدم
- خذ بعين الاعتبار الموقع الجغرافي إذا ذكره المستخدم
- رتب النتائج من الأنسب للأقل
- إذا ما في 3 بوستات مناسبة، رشح الموجود فقط

أجب بهذا الشكل بالضبط لكل بوست:
PostID: [رقم]
العنوان: [عنوان البوست]
سبب الاختيار: [سبب واضح ومختصر]
---
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
