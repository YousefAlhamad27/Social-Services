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
        private readonly PostRepository _postRepo;

        public clsAiRecommendationService(PostRepository postRepo, IConfiguration configuration)
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
أنت مساعد متخصص في توصية الخدمات.

طلب المستخدم: {userMessage}

البوستات المتاحة:
{postsJson}

قواعد:
- رشح البوستات الأنسب حسب نوع الخدمة والموقع
- إذا ذكر المستخدم عدد معين التزم به، وإلا رشح 3
- رتب من الأنسب للأقل

أرجع فقط JSON بهذا الشكل بدون أي نص إضافي:
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
